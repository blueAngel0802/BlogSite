namespace BlogApp.Api.DTOs.Like;

public class LikeStatusDto
{
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool? UserReaction { get; set; }
}