using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Settings;

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

            var settings = scope.ServiceProvider.GetService<IOptions<AppSettings>>().Value;
            var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher<UserEntity>>();

            signalsContext.Users.Add(new UserEntity
            {
                Username = settings.Administration.DefaultUsername,
                PasswordHash = passwordHasher.HashPassword(null, settings.Administration.DefaultPassword),
                IsAdmin = true,
                IsDisabled = false
            });

            signalsContext.SaveChanges();
        }
    }
}
