using System.ComponentModel.DataAnnotations;

namespace BlogApp.Api.Models.Entities; 

public class Like
{
    public Guid Id { get; set; }

    public Guid BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public bool IsLike { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}