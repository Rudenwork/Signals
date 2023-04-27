namespace Signals.App.Common
{
    public enum TimeUnit
    {
        Second,
        Minute,
        Hour,
        Day
    }

    static class TimeUnitExtensions
    {
        public static TimeSpan GetTimeSpan(this TimeUnit? timeUnit, int? length = 1)
        {
            int lengthValue = length ?? 0;

            return timeUnit switch
            {
                TimeUnit.Second => TimeSpan.FromSeconds(lengthValue),
                TimeUnit.Minute => TimeSpan.FromMinutes(lengthValue),
                TimeUnit.Hour => TimeSpan.FromHours(lengthValue),
                TimeUnit.Day => TimeSpan.FromDays(lengthValue),
                _ => TimeSpan.Zero
            };
        }

        public static TimeSpan GetTimeSpan(this TimeUnit timeUnit, int? length = 1)
        {
            return timeUnit.GetTimeSpan(length);
        }
    }
}
