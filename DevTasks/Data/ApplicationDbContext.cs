using DevTasks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTasks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>()
                .HasOne(d => d.State)
                .WithMany(s => s.Districts)
                .HasForeignKey(d => d.StateId);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.State)
                .WithMany(s => s.Customers)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.District)
                .WithMany(d => d.Customers)
                .HasForeignKey(c => c.DistrictId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade

            base.OnModelCreating(modelBuilder);
        }

    }
}
