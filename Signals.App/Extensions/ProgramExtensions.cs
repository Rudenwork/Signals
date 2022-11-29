using Microsoft.AspNetCore.Identity;
using Signals.App.Database.Entities;
using Signals.App.Database;
using Microsoft.EntityFrameworkCore;
using Signals.App.Services;
using Signals.App.Jobs;

namespace Signals.App.Extensions
{
    public static class ProgramExtensions
    {
        public static void PrepareDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var signalsContext = scope.ServiceProvider.GetService<SignalsContext>();

            signalsContext.Database.Migrate();

            signalsContext.SignalExecutions
                .Where(x => x.StageScheduledOn.AddMinutes(1) < DateTime.UtcNow)
                .ExecuteDelete();

            signalsContext.SignalExecutions
                .Where(x => x.IsActive)
                .ExecuteUpdate(x => x.SetProperty(x => x.IsActive, false));

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

        public static void ScheduleJobs(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var signalsContext = scope.ServiceProvider.GetService<SignalsContext>();
            var jobService = scope.ServiceProvider.GetService<JobService>();

            var signals = signalsContext.Signals
                .Where(x => !x.IsDisabled)
                .ToList();

            foreach (var signal in signals)
            {
                jobService.Schedule<SignalJob>(signal.Id, signal.Schedule).Wait();
            }

            var signalExecutions = signalsContext.SignalExecutions.ToList();

            foreach (var signalExecution in signalExecutions)
            {
                jobService.Schedule<StageJob>(signalExecution.SignalId, signalExecution.StageScheduledOn).Wait();
            }
        }
    }
}
