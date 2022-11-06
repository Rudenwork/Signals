using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Identity;

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

builder.Services.AddAuthentication()
    .AddIdentityCookies();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseIdentityServer();
app.MapControllers();

app.Run();