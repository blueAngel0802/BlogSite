namespace BlogApp.Api.DTOs.Like;

public class ToggleLikeDto
{
    public Guid BlogPostId { get; set; }
    public bool IsLike { get; set; }
}