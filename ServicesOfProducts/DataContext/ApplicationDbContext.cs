using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.Models;

namespace ServicesOfProducts.DataContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Gender)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .Navigation(o => o.User)
            .AutoInclude();
        
        modelBuilder.Entity<Product>()
            .Navigation(p => p.Category)
            .AutoInclude();

        modelBuilder.Entity<Transaction>()
            .Navigation(t => t.Order)
            .AutoInclude();

        modelBuilder.Entity<Transaction>()
            .Navigation(t => t.Product)
            .AutoInclude();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}