using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tweet.Models;

namespace Tweet.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Like> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followee)
            .WithMany()
            .HasForeignKey(f => f.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany()
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}