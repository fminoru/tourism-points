using Microsoft.EntityFrameworkCore;
using TourismPoints.Domain.Entities;

namespace TourismPoints.Infrastructure.Context;

public class TourismDbContext : DbContext
{
    public TourismDbContext(DbContextOptions<TourismDbContext> options) : base(options)
    {
    }

    public DbSet<TouristPoint> TouristPoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TouristPoint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.City).IsRequired();
            entity.Property(e => e.State).IsRequired().HasMaxLength(2);
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}