using Microsoft.EntityFrameworkCore;
using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;

namespace Signals.App.Core.Jobs
{
    public class SignalJob : IJob
    {
        private SignalsContext SignalsContext { get; set; }

        public SignalJob(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var id = Guid.Parse(context.MergedJobDataMap[nameof(SignalEntity.Id)].ToString());

            var signal = await SignalsContext.Signals.FirstAsync(x => x.Id == id);

            Console.WriteLine($"[{nameof(SignalJob)}] - {signal.Name} ({signal.Id})");
        }
    }
}
