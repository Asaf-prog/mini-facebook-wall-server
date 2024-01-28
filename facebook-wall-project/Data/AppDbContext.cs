using facebook_wall_project.Models;
using Microsoft.EntityFrameworkCore;

namespace facebook_wall_project.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    public DbSet<User>? Users {get; set;}
    public DbSet<Post>? Posts {get; set;}
    public DbSet<Like>? Likes {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Like>()
        .HasKey(l => l.LikeId);

         modelBuilder.Entity<User>()
        .HasIndex(u => u.UserName)
        .IsUnique();
    }
}
