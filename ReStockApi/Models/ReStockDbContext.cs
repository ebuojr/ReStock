using Microsoft.EntityFrameworkCore;

namespace ReStockApi.Models
{
    public class ReStockDbContext : DbContext
    {
        public ReStockDbContext(DbContextOptions<ReStockDbContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        public DbSet<DistributionCenterInventory> DistributionCenterInventories { get; set; }
        public DbSet<InventoryThreshold> InventoryThresholds { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        public DbSet<Reorder> Reorders { get; set; }
        public DbSet<ReOrderLog> ReOrderLogs { get; set; }
        public DbSet<JobLastRun> JobLastRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasIndex(p => p.ItemNo).IsUnique();

            modelBuilder.Entity<Store>().HasKey(s => s.Id);
            modelBuilder.Entity<Store>().HasIndex(s => s.No).IsUnique();

            modelBuilder.Entity<StoreInventory>().HasKey(si => si.Id);
            modelBuilder.Entity<StoreInventory>().HasIndex(si => new { si.StoreNo, si.ItemNo }).IsUnique();

            modelBuilder.Entity<DistributionCenterInventory>().HasKey(dci => dci.Id);
            modelBuilder.Entity<DistributionCenterInventory>().HasIndex(dci => dci.ItemNo).IsUnique();

            modelBuilder.Entity<InventoryThreshold>().HasKey(it => it.Id);
            modelBuilder.Entity<InventoryThreshold>().HasIndex(it => new { it.StoreNo, it.ItemNo }).IsUnique();

            modelBuilder.Entity<SalesOrder>().HasKey(so => so.Id);
            modelBuilder.Entity<SalesOrder>().HasIndex(so => so.HeaderNo).IsUnique();

            modelBuilder.Entity<SalesOrderLine>().HasKey(sol => sol.Id);
            modelBuilder.Entity<SalesOrderLine>().HasIndex(sol => new { sol.HeaderNo, sol.LineNo }).IsUnique();

            modelBuilder.Entity<Reorder>().HasKey(r => r.Id);
            modelBuilder.Entity<Reorder>().HasIndex(r => new { r.StoreNo, r.ItemNo }).IsUnique();

            modelBuilder.Entity<ReOrderLog>().HasKey(rol => rol.Id);
            modelBuilder.Entity<JobLastRun>().HasKey(jlr => jlr.Id);
        }
    }
}
