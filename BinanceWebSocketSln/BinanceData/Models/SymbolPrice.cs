namespace BinanceData.Models;

public class SymbolPrice
{
    public long Id { get; set; }
    public string Symbol { get; set; }
    public DateTime EventTime { get; set; }
    public decimal Price { get; set; }
    
}