namespace BinanceWebApi.Utils;

public static class TimePeriodUtils
{
    public static readonly IDictionary<string, TimeSpan> AllowedTimePeriods = new Dictionary<string, TimeSpan>()
    {
        { "1w", TimeSpan.FromDays(7) },
        { "1d", TimeSpan.FromDays(1) },
        { "30m", TimeSpan.FromMinutes(30) },
        { "5m", TimeSpan.FromMinutes(5) },
        { "1m", TimeSpan.FromMinutes(1) },
    };

    public static TimeSpan GetTimePeriod(string timePeriod)
    {
        if (AllowedTimePeriods.TryGetValue(timePeriod, out var period))
        {
            return period;
        }

        throw new NotImplementedException("Given time period is not allowed");
    }
}