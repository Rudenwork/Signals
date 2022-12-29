using Binance.Net;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Extensions;
using Signals.App.Identity;
using Signals.App.Services;
using Signals.App.Settings;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddDbContext<SignalsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(Signals))));

builder.Services.AddIdentityServer(options => options.KeyManagement.KeyPath = $"{AppContext.BaseDirectory}/keys")
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = nameof(Client).ToLower(),
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            },
            AllowOfflineAccess = true,
            RequireClientSecret = false
        }
    })
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = settings.Identity.Authority;
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName.Replace("+", "."));

    options.AddSecurityDefinition(nameof(SecuritySchemeType.OpenIdConnect), new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{settings.Identity.Authority}/connect/token")
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = nameof(SecuritySchemeType.OpenIdConnect)
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddMapster(options =>
{
    options.MapEnumByName = true;
    options.IgnoreNullValues = true;
});

builder.Services.AddMassTransit(options =>
{
    options.SetNetEndpointNameFormatter();
    options.AddConsumers(Assembly.GetExecutingAssembly());

    options.UsingRabbitMq((context, config) =>
    {
        config.Host(settings.RabbitMq.Host, "/", host =>
        {
            host.Username(settings.RabbitMq.Username);
            host.Password(settings.RabbitMq.Password);
        });

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediator(options =>
{
    options.AddConsumers(Assembly.GetExecutingAssembly());
});

builder.Services.AddQuartz(options =>
{
    options.UsePersistentStore(config =>
    {
        config.UseSqlServer(builder.Configuration.GetConnectionString($"{nameof(Signals)}.{nameof(Quartz)}"));
        config.UseJsonSerializer();
    });

    options.MisfireThreshold = TimeSpan.FromMinutes(1);
    options.UseMicrosoftDependencyInjectionJobFactory();
});

builder.Services.AddQuartzServer();

builder.Services.AddBinance();

builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
builder.Services.AddScoped<Scheduler>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.PrepareDatabase();

app.Run();