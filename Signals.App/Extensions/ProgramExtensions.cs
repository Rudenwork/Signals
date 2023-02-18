using MassTransit;
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
            var quartzContext = scope.ServiceProvider.GetService<QuartzContext>();

            if (!quartzContext.Database.CanConnect())
            {
                quartzContext.Database.EnsureCreated();

                var sql = File.ReadAllText("Database/Scripts/Signals.Quartz-Init.sql");

                quartzContext.Database.ExecuteSqlRaw(sql);
            }

            signalsContext.Database.MigrateAsync().Wait();

            if (signalsContext.Users.Any(x => x.IsAdmin))
                return;

            var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher<UserEntity>>();

            signalsContext.Users.Add(new UserEntity
            {
                Id = Guid.Parse("78911115-ed98-4e96-c6b1-08dae7620d69"),
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "admin"),
                IsAdmin = true,
                IsDisabled = false
            });

            signalsContext.SaveChanges();
        }
    }
}
