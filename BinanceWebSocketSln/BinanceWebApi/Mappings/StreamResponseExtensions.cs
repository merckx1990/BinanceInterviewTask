using System.Globalization;
using BinanceApiConnector;
using BinanceData.Models;

namespace BinanceWebApi.Mappings;

public static class StreamResponseExtensions
{
    public static SymbolPrice ToSymbolPriceEntity(this StreamData value)
    {
        var price = decimal.Parse(value.Price, new NumberFormatInfo() { NumberDecimalSeparator = "."});
        
        return new SymbolPrice()
        {
            Price = price,
            Symbol = value.Symbol.ToLower(),
            EventTime = UnixMillisecondsToDateTime(value.EventTime)
        };
    }
    
    public static DateTime UnixMillisecondsToDateTime(long timestamp, bool local = false)
    {
        var offset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        return local ? offset.LocalDateTime : offset.UtcDateTime;
    }
}