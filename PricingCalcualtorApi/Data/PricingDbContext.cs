using Microsoft.EntityFrameworkCore;
using PricingCalcualtorApi.Models;

namespace PricingCalcualtorApi.Data
{
    public class PricingDbContext : DbContext
    {
        public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Service>()
                .HasKey(s => s.ServiceId);

            modelBuilder.Entity<Price>()
                .HasKey(p => p.PriceId);

            modelBuilder.Entity<Price>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Prices)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<Price>()
                .HasOne(p => p.Service)
                .WithMany(s => s.Prices)
                .HasForeignKey(p => p.ServiceId);

            modelBuilder.Entity<Price>()
                .Property(p => p.BasePrice)
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(p => p.Discount)
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(p => p.StartDate)
                .IsRequired();

            modelBuilder.Entity<Price>()
                .Property(p => p.EndDate)
                .IsRequired();
        }
    }
}
