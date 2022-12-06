using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Commands.Signal;
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

            signalsContext.SignalExecutions
                .Where(x => x.StageScheduledOn.AddMinutes(1) < DateTime.UtcNow)
                .ExecuteDelete();

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
            var commandService = scope.ServiceProvider.GetService<CommandService>();

            var signals = signalsContext.Signals
                .Where(x => !x.IsDisabled)
                .ToList();

            foreach (var signal in signals)
            {
                commandService.ScheduleRecurring(new StartSignal.Command { SignalId = signal.Id }, signal.Schedule, signal.Id).Wait();
            }

            var signalExecutions = signalsContext.SignalExecutions.ToList();

            foreach (var signalExecution in signalExecutions)
            {
                commandService.Schedule(new ExecuteStage.Command { SignalId = signalExecution.SignalId }).Wait();
            }
        }
    }
}
