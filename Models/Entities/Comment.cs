using System.ComponentModel.DataAnnotations;

namespace BlogApp.Api.Models.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public Guid BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }  

    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public Guid? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}