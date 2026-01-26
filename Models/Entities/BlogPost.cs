using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BlogApp.Api.Models.Entities;

public class BlogPost
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid AuthorId { get; set; }

    public User Author { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Like> Likes { get; set; }
}