using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Signals.App.Database.Extentions;
using Microsoft.OpenApi.Models;
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
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri($"{settings.Identity.Authority}/.well-known/openid-configuration")
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