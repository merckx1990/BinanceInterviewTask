using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace BinanceApiConnector;

public class BinanceClient
{
    public BinanceClient()
    {
    }

    public WebsocketClient Create(string url)
    {
        var client = new WebsocketClient(new Uri(url));
        client.ReconnectTimeout = TimeSpan.FromSeconds(30);
        return client;
    }
}