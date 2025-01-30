using Inlämningsuppgift_Webshop;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Webshop.Models;

internal class AdvNookContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketProduct> BasketProduct { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BasketProduct>()
            .HasKey(bp => bp.Id);  // Sätt primärnyckel på BasketProduct

        modelBuilder.Entity<BasketProduct>()
            .HasOne(bp => bp.Basket)
            .WithMany(b => b.BasketProducts)
            .HasForeignKey(bp => bp.BasketId);

        modelBuilder.Entity<BasketProduct>()
            .HasOne(bp => bp.Product)
            .WithMany(p => p.BasketProducts)
            .HasForeignKey(bp => bp.ProductId);

        // Optional: Configure the enum property if needed
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>(); // Store enum as string in the database
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=tcp:slayserver.database.windows.net,1433;Initial Catalog=Slay Stina;Persist Security Info=False;User ID=slaystina;Password=August1337;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

}
