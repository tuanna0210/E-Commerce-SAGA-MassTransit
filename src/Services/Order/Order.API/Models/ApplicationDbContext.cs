using Microsoft.EntityFrameworkCore;
using Order.API.SAGA;

namespace Order.API.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<ECommerceSagaData> SagaData { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(o => o.Id);

        modelBuilder.Entity<ECommerceSagaData>().HasKey(s => s.CorrelationId);
    }
}
