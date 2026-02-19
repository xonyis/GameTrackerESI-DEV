using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Data;

public class GameTrackerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Studio> Studios { get; set; }
    public DbSet<PlaySession> PlaySessions { get; set; }

    public GameTrackerDbContext()
    {
    }

    public GameTrackerDbContext(DbContextOptions<GameTrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=gametracker.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relation Many-to-Many Game ↔ Studio
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Studios)
            .WithMany(s => s.Games);

        // Relation Many-to-Many Game ↔ Category
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Categories)
            .WithMany(c => c.Games);

        // PlaySession → Game et User (Many-to-One)
        modelBuilder.Entity<PlaySession>()
            .HasOne(ps => ps.Game)
            .WithMany(g => g.PlaySessions)
            .HasForeignKey(ps => ps.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlaySession>()
            .HasOne(ps => ps.User)
            .WithMany(u => u.PlaySessions)
            .HasForeignKey(ps => ps.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
