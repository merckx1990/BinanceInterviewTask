using BinanceData.Models;
using Microsoft.EntityFrameworkCore;

namespace BinanceData;

public class BinanceStreamDbContext : DbContext
{
    public BinanceStreamDbContext(DbContextOptions<BinanceStreamDbContext> options)
        : base(options)
    { }

    public DbSet<SymbolPrice> SymbolPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SymbolPrice>()
            .HasKey(b => b.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}