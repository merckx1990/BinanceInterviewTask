using BinanceWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BinanceWebApi.Controllers;

[ApiController]
[Route("api")]
public class BinancePriceApiController : ControllerBase
{
 
    private readonly ILogger<BinancePriceApiController> _logger;
    private readonly IPriceService _priceService;

    public BinancePriceApiController(ILogger<BinancePriceApiController> logger, IPriceService priceService)
    {
        _logger = logger;
        _priceService = priceService;
    }

    /// <summary>
    /// GET /api/{symbol}/24hAvgPrice - Returns the average price for the last 24h of data in the database ( or the oldest available, if 24h of data is not available )
    /// </summary>
    /// <returns></returns>
    [HttpGet("{symbol}/24hAvgPrice")]
    public async Task<IActionResult> Get24hAvgPriceAsync([FromRoute] string symbol)
    {
        var avgPrice = await _priceService.Get24hAvgPrice(symbol);
        return Ok(new {avgPrice});
    }
    
    [HttpGet("{symbol}/SimpleMovingAverage")]
    public async Task<IActionResult> GetSimpleMovingAverageAsync([FromRoute] string symbol, 
        [FromQuery(Name = "n")] int numberOfDataPoints, [FromQuery(Name = "p")] string timePeriod, [FromQuery(Name = "s")] DateTime? startDateTime)
    {
        //TODO Waiting impl
        return Ok("24h price " + numberOfDataPoints + startDateTime + timePeriod);
    }
}