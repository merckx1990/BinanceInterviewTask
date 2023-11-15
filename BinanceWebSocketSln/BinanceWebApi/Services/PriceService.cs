using BinanceData;
using BinanceWebApi.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BinanceWebApi.Services;

public class PriceService : IPriceService
{
    private readonly BinanceStreamDbContext _context;
    private readonly IMemoryCache _cache;

    public PriceService(BinanceStreamDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<decimal> Get24hAvgPrice(string symbol)
    {
        var symbolPrices = _context.SymbolPrices.AsNoTracking();
        var lowerCaseSymbol = symbol.ToLower();
        var timeBefore24H = DateTime.UtcNow.AddDays(-1);
        
        if (_cache.TryGetValue((symbol, timeBefore24H), out var result))
        {
            return (decimal)result;
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions {
            AbsoluteExpiration = DateTime.Now.AddMinutes(60),
            SlidingExpiration = TimeSpan.FromMinutes(2),
            Size = 1024,
        };
        
        if (await symbolPrices.AnyAsync(x => x.Symbol.Equals(lowerCaseSymbol) && x.EventTime < timeBefore24H))
        {
            var avgPrice = await symbolPrices
                .Where(x => x.Symbol.Equals(lowerCaseSymbol) && x.EventTime >= timeBefore24H)
                .AverageAsync(x => x.Price);
            
            _cache.Set((symbol, timeBefore24H), avgPrice, cacheEntryOptions);
            return avgPrice;
        }

        var firstSymbolPrice = (await symbolPrices.OrderBy(x => x.EventTime).FirstOrDefaultAsync())?.Price ?? Decimal.Zero;
        _cache.Set((symbol, timeBefore24H), firstSymbolPrice, cacheEntryOptions);
        
        return firstSymbolPrice;
    }

    public async Task<decimal> GetSMAPrice(string symbol, int numberOfDataPoints, string timePeriod, DateTime? startDateTime)
    {
        var symbolPrices = _context.SymbolPrices.AsNoTracking();
        var lowerCaseSymbol = symbol.ToLower();
        var startDate = startDateTime ?? DateTime.UtcNow;
        
        var period = startDate.AddMinutes(-numberOfDataPoints * TimePeriodUtils.GetTimePeriod(timePeriod).TotalMinutes);

        var symbolPricesList = symbolPrices.Where(x => x.Symbol.Equals(lowerCaseSymbol) && x.EventTime >= period && x.EventTime <= startDate)
            .AsNoTracking().ToList();
        
        //TODO Additional questions arised 
        return 0;
    }
}