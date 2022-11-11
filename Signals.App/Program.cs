using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Extentions;
using Signals.App.Identity;
using Signals.App.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.Get<Settings>();

builder.Services.AddDbContext<SignalsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(Signals))));

builder.Services.AddIdentityCore<UserEntity>()
    .AddSignInManager<SignInManager<UserEntity>>()
    .AddUserStore<UserStore>();

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResource()
        {
            Name = "api",
            UserClaims =
            { 
                JwtClaimTypes.Subject, 
                JwtClaimTypes.PreferredUserName, 
                JwtClaimTypes.Role 
            }
        }
    })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = nameof(Client).ToLower(),
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = { "api" },
            AllowOfflineAccess = true,
            RequireClientSecret = false
        }
    })
    .AddAspNetIdentity<UserEntity>()
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.PrepareDatabase();

app.Run();