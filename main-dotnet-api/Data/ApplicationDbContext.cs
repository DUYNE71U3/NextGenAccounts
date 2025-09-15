using Microsoft.EntityFrameworkCore;
using main_dotnet_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace main_dotnet_api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.GameTitle).HasMaxLength(100);
                entity.Property(e => e.Server).HasMaxLength(50);
                entity.Property(e => e.AccountLevel).HasMaxLength(100);
                entity.Property(e => e.AccountDetails).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);

                // Configure relationship
                entity.HasOne(e => e.Category)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.CategoryId);
            });

            // Configure Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Configure relationship with ApplicationUser
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
            });

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.AddedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                // Configure relationship with Cart
                entity.HasOne(e => e.Cart)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure relationship with Product
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CartId);
                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => new { e.CartId, e.ProductId }).IsUnique(); // Prevent duplicate items
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();

                // Configure relationship with ApplicationUser
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.Status);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();

                // Configure relationship with Order
                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure relationship with Product
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.ProductId);
            });

            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
            });

            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );

            // Seed initial data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "MMORPG", Description = "Massively Multiplayer Online Role-Playing Games" },
                new Category { Id = 2, Name = "MOBA", Description = "Multiplayer Online Battle Arena Games" },
                new Category { Id = 3, Name = "FPS", Description = "First-Person Shooter Games" },
                new Category { Id = 4, Name = "Mobile Games", Description = "Mobile Gaming Accounts" }
            );

        }
    }
}