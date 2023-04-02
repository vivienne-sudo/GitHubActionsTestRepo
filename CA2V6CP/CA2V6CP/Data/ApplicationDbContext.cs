using CA2V6CP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CA2V6CP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Golfer> Golfers { get; set; }

        public DbSet<TeeTimeBooking> TeeTimeBookings { get; set; }

        public DbSet<Register> Registers { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Register>()
            .HasKey(r => r.Email);

            modelBuilder.Entity<Register>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Register>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<Golfer>().ToTable("Golfers");
            modelBuilder.Entity<TeeTimeBooking>().ToTable("TeeTimeBookings");
        }

    }
}
