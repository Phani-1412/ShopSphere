using Microsoft.EntityFrameworkCore;
using ShopSphere.Models;

namespace ShopSphere.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentReference> PaymentReferences { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SellerStore> SellerStores { get; set; }
        public DbSet<MarketplaceReport> MarketplaceReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Seller>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Seller>(s => s.UserID);

            modelBuilder.Entity<SellerStore>()
                .HasOne(s=>s.Seller)
                .WithMany(s=>s.SellerStores)
                .HasForeignKey(s=>s.SellerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductAttribute>()
                .HasOne(pa => pa.Product)
                .WithMany(p => p.Attributes)
                .HasForeignKey(pa => pa.ProductID);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderID);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipment)
                .WithOne(s => s.Order)
                .HasForeignKey<Shipment>(s => s.OrderID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ReturnRequests)
                .WithOne(r => r.Order)
                .HasForeignKey(r => r.OrderID);

            modelBuilder.Entity<ReturnRequest>()
                .HasOne(r => r.Refund)
                .WithOne(f => f.ReturnRequest)
                .HasForeignKey<Refund>(f => f.ReturnID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Disputes)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderID);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispute>()
                .HasOne(d => d.RaisedByUser)
                .WithMany(u => u.DisputesRaised)
                .HasForeignKey(d => d.RaisedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
