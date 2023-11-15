namespace BinanceWebApi.Services;

public interface IPriceService
{
    Task<decimal> Get24hAvgPrice(string symbol);
    Task<decimal> GetSMAPrice(string symbol, int numberOfDataPoints, string timePeriod, DateTime? startDateTime);
}