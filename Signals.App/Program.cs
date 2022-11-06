using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

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
        options.Authority = builder.Configuration["ASPNETCORE_URLS"]
           .Split(";")
           .First(u => u.Contains("https"))
           .Replace("*", "localhost");

        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();