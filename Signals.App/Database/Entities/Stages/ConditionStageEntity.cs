namespace Signals.App.Database.Entities.Stages
{
    public class ConditionStageEntity : StageEntity
    {
        public int? RetryCount { get; set; }
        public TimeSpan? RetryDelay { get; set; }

        //EF Injected
        public BlockEntity Block { get; set; }
    }
}
