using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Extensions
{
    public static class ProgramExtensions
    {
        public static void PrepareDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var signalsContext = scope.ServiceProvider.GetService<SignalsContext>();

            signalsContext.Database.Migrate();

            if (signalsContext.Users.Any(x => x.IsAdmin))
                return;

            var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher<UserEntity>>();

            signalsContext.Users.Add(new UserEntity
            {
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "admin"),
                IsAdmin = true,
                IsDisabled = false
            });

            signalsContext.SaveChanges();
        }
    }
}
