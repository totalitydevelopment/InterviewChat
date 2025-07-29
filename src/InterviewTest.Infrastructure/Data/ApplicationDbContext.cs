using Microsoft.EntityFrameworkCore;
using InterviewTest.Core.Entities;
using InterviewTest.Core.ValueObjects;

namespace InterviewTest.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Guest> Guests { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Guest entity
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            
            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(50);
                address.Property(a => a.PostalCode).HasMaxLength(20);
                address.Property(a => a.Country).HasMaxLength(100);
            });
        });

        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.RoomRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TaxRate).HasColumnType("decimal(5,4)");

            // Configure relationship
            entity.HasOne(e => e.Guest)
                  .WithMany(g => g.Bookings)
                  .HasForeignKey(e => e.GuestId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data - removed for simplicity, will use seeder service
    }
}