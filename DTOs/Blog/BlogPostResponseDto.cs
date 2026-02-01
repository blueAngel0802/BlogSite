namespace BlogApp.Api.DTOs.Blog;

public class BlogPostResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public string AuthorUsername { get; set; }
    public DateTime CreatedAt { get; set; }
}