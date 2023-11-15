using Newtonsoft.Json;

namespace BinanceApiConnector;

public class StreamResponse
{
    [JsonProperty("stream")] 
    public string Stream { get; set; }

    [JsonProperty("data")] 
    public StreamData Data { get; set; }
}

public class StreamData
{
    [JsonProperty("e")] 
    public string EventType { get; set; }

    [JsonProperty("E")] 
    public long EventTime { get; set; }

    [JsonProperty("s")] 
    public string Symbol { get; set; }

    [JsonProperty("a")] 
    public long AggTradeId { get; set; }

    [JsonProperty("p")] 
    public string Price { get; set; }

    [JsonProperty("q")] 
    public string Quantity { get; set; }

    [JsonProperty("f")] 
    public long FirstTradeId { get; set; }

    [JsonProperty("l")] 
    public long LastTradeId { get; set; }

    [JsonProperty("T")]
    public long TradeTime { get; set; }
}