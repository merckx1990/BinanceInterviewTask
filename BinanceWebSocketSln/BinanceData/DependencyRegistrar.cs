using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BinanceData;

public static class DependencyRegistrar
{
    public static IServiceCollection RegisterBinanceApiDb(this IServiceCollection serviceCollection, string connString)
    {
        serviceCollection.AddDbContext<BinanceStreamDbContext>(options => options.UseSqlServer(connString));

        return serviceCollection;
    }

}