using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Jobs.Extensions
{
    public static class JobExtensions
    {
        public static void ScheduleJobs(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var signalsContext = scope.ServiceProvider.GetService<SignalsContext>();
            var jobService = scope.ServiceProvider.GetService<JobService>();

            var signals = signalsContext.Signals
                .Where(x => !x.IsDisabled)
                .ToList();

            jobService.ScheduleSignals(signals).Wait();
        }
    }
}
