using Microsoft.EntityFrameworkCore;

namespace s24422_zad5.Models;

public class TripContext(DbContextOptions<TripContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientTrip> ClientTrips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientTrip>()
            .HasKey(ct => new { ct.ClientId, ct.TripId });

        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Client)
            .WithMany(c => c.ClientTrips)
            .HasForeignKey(ct => ct.ClientId);

        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Trip)
            .WithMany(t => t.ClientTrips)
            .HasForeignKey(ct => ct.TripId);

        modelBuilder.Entity<Country>()
            .HasOne(c => c.Trip)
            .WithMany(t => t.Countries)
            .HasForeignKey(c => c.TripId);
    }
}