namespace BlogApp.Api.DTOs.Comment;

public class CreateCommentDto
{
    public Guid BlogPostId { get; set; }
    public required string Content { get; set; }
    public Guid? ParentCommentId { get; set; }
}