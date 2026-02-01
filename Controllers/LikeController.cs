using System.Security.Claims;
using BlogApp.Api.Data;
using BlogApp.Api.DTOs.Like;
using BlogApp.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Api.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [Authorize]
        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromForm] ToggleLikeDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var existing = await _context.Likes
            .FirstOrDefaultAsync(like =>
                like.BlogPostId == dto.BlogPostId &&
                like.UserId == userId
            );

            if (existing == null)
            {
                _context.Likes.Add(new Like
                {
                    BlogPostId = dto.BlogPostId,
                    IsLike = dto.IsLike,
                    UserId = userId
                });
            }
            else
            {
                if (existing.IsLike == dto.IsLike)
                {
                    _context.Likes.Remove(existing);
                }
                else
                {
                    existing.IsLike = dto.IsLike;
                    _context.Likes.Update(existing);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("blog/{blogPostId}")]
        public async Task<IActionResult> GetStatus(Guid blogPostId)
        {
            Guid? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }

            var likes = _context.Likes
            .Where(like => like.BlogPostId == blogPostId)
            .ToList();

            var result = new LikeStatusDto
            {
                Likes = likes.Count(like => like.IsLike == true),
                Dislikes = likes.Count(like => like.IsLike == false),
                UserReaction = userId == null
                ? null
                : likes.FirstOrDefault(l => l.UserId == userId)?.IsLike
            };

            return Ok(result);
        }
    }
}
