using System.ComponentModel.DataAnnotations;

namespace BlogApp.Api.Models.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BlogPost> BlogPosts { get; set; }
}
