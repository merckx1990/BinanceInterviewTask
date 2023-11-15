using BinanceApiConnector;
using BinanceData;
using BinanceWebApi.Mappings;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;

namespace BinanceWebApi.Services;

public class BinanceWorker : BackgroundService
{
    private readonly ILogger<BinanceWorker> _logger;
    private readonly BinanceClient _client;
    private readonly IServiceProvider _services;

    public BinanceWorker(ILogger<BinanceWorker> logger, BinanceClient client, IServiceProvider services)
    {
        _logger = logger;
        _client = client;
        _services = services;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        using var webSocketClient = _client.Create(
            "wss://stream.binance.com:9443/stream?streams=btcusdt@aggTrade/adausdt@aggTrade/ethusdt@aggTrade");

        using var scope = _services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BinanceStreamDbContext>();
        webSocketClient.ReconnectionHappened.Subscribe(info =>
            _logger.LogInformation($"Reconnection happened, type: {info.Type}"));
        
        webSocketClient.MessageReceived.Subscribe(msg =>
        {
            if (msg.Text != null)
            {
                _logger.LogInformation($"Message received: {msg}");
                var msgObj = JsonConvert.DeserializeObject<StreamResponse>(msg.Text);
                
                //TODO batching mechanism to improve inserts 
                var symbolPrice = msgObj.Data.ToSymbolPriceEntity();
                dbContext.SymbolPrices.Add(symbolPrice);
                dbContext.SaveChanges();
            }
        });
        await webSocketClient.Start();
        
        WaitHandle.WaitAny(new[] { stoppingToken.WaitHandle });
    }
}