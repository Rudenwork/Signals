using Quartz;

namespace Signals.App.Core.Jobs
{
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.Beep();
            Console.WriteLine("JOB EXECUTION WITH KEY: " + context.MergedJobDataMap["key"]);
            return Task.CompletedTask;
        }
    }
}
