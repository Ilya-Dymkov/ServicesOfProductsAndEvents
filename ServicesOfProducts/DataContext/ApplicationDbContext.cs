using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.Models;

namespace ServicesOfProducts.DataContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Navigation(p => p.Category)
            .AutoInclude();

        modelBuilder.Entity<Transaction>()
            .Navigation(t => t.Product)
            .AutoInclude();

        modelBuilder.Entity<Transaction>()
            .Navigation(t => t.Order)
            .AutoInclude();
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}