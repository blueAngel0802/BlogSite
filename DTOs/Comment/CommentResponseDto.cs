namespace BlogApp.Api.DTOs.Comment;

public class CommentResponseDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }

    public string AuthorUsername {get; set;}
    public DateTime CreatedAt { get; set; }

    public Guid? ParentCommentId { get; set; }
}