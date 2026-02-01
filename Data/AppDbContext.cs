using Microsoft.EntityFrameworkCore;
using BlogApp.Api.Models.Entities;

namespace BlogApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>()
            .HasMany(u => u.BlogPosts)
            .WithOne(bp => bp.Author)
            .HasForeignKey(bp => bp.AuthorId);

        modelBuilder.Entity<BlogPost>()
            .HasMany(bp => bp.Comments)
            .WithOne(c => c.BlogPost)
            .HasForeignKey(c => c.BlogPostId);

        modelBuilder.Entity<BlogPost>()
            .HasMany(bp => bp.Likes)
            .WithOne(l => l.BlogPost)
            .HasForeignKey(l => l.BlogPostId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany()
            .HasForeignKey(c => c.ParentCommentId);

        modelBuilder.Entity<Like>()
            .HasIndex(l => new { l.BlogPostId, l.UserId })
            .IsUnique();
    }
}