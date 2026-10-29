using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;

namespace PersonalPlans.Infrastructure.Persistence;

public class PersonalPlansContext : DbContext
{
    public PersonalPlansContext(DbContextOptions<PersonalPlansContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Recommendation> Recommendations => Set<Recommendation>();
    public DbSet<RecommendationActivity> RecommendationActivities =>
        Set<RecommendationActivity>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
            entity.Property(u => u.Name).HasMaxLength(150).IsRequired();
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(30).IsRequired();
        });

        // Activities
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasIndex(a => a.Name).IsUnique();
            entity.Property(a => a.Name).HasMaxLength(150).IsRequired();
            entity.Property(a => a.Category).HasMaxLength(100).IsRequired();
            entity.Property(a => a.CostPerPerson).HasPrecision(10, 2);
            entity.Property(a => a.SuitableCompanies).HasMaxLength(100).IsRequired();
        });

        // Favorites
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.HasIndex(f => new { f.UserId, f.ActivityId }).IsUnique();

            entity.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Activity)
                .WithMany()
                .HasForeignKey(f => f.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Recommendations
        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Budget).HasPrecision(10, 2);
            entity.Property(r => r.TotalCost).HasPrecision(10, 2);
            entity.Property(r => r.Company).HasMaxLength(30).IsRequired();

            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Activities included in each recommendation
        modelBuilder.Entity<RecommendationActivity>(entity =>
        {
            entity.HasKey(ra => ra.Id);
            entity.HasIndex(ra => new { ra.RecommendationId, ra.ActivityId })
                .IsUnique();

            entity.Property(ra => ra.Cost).HasPrecision(10, 2);

            entity.HasOne(ra => ra.Recommendation)
                .WithMany(r => r.RecommendationActivities)
                .HasForeignKey(ra => ra.RecommendationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ra => ra.Activity)
                .WithMany()
                .HasForeignKey(ra => ra.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Ratings
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.UserId, r.ActivityId }).IsUnique();

            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Activity)
                .WithMany()
                .HasForeignKey(r => r.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
