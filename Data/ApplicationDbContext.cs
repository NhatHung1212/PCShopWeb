using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Models;

namespace ShopWeb.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map tên bảng Laravel (chữ thường, số nhiều)
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Review>().ToTable("reviews");
        modelBuilder.Entity<CartItem>().ToTable("carts");
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<OrderDetail>().ToTable("order_items");

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.User)
            .WithMany()
            .HasForeignKey(ci => ci.UserId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.Price)
            .HasPrecision(18, 2);
        
        // Category relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Review relationships
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Laptops", Description = "Powerful laptops for work and gaming", IconClass = "fa-laptop", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 2, Name = "Desktops", Description = "High-performance desktop computers", IconClass = "fa-desktop", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 3, Name = "Components", Description = "PC parts and components", IconClass = "fa-microchip", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 4, Name = "Peripherals", Description = "Keyboards, mice, and accessories", IconClass = "fa-keyboard", CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 5, Name = "Monitors", Description = "Displays and monitors", IconClass = "fa-tv", CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}
