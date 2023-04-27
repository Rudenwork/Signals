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
        public static TimeSpan GetTimeSpan(this TimeUnit? timeUnit, int? length)
        {
            if (timeUnit is null || length is null)
            {
                return TimeSpan.Zero;
            }

            return timeUnit.Value.GetTimeSpan(length.Value);
        }

        public static TimeSpan GetTimeSpan(this TimeUnit timeUnit, int length)
        {
            return timeUnit switch
            {
                TimeUnit.Second => TimeSpan.FromSeconds(length),
                TimeUnit.Minute => TimeSpan.FromMinutes(length),
                TimeUnit.Hour => TimeSpan.FromHours(length),
                TimeUnit.Day => TimeSpan.FromDays(length)
            };
        }
    }
}
