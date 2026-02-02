using System.Security.Claims;
using BlogApp.Api.Data;
using BlogApp.Api.DTOs.Comment;
using BlogApp.Api.DTOs.Common;
using BlogApp.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub"));

            var blogPost = await _context.BlogPosts.AnyAsync(bp => bp.Id == dto.BlogPostId);

            if (!blogPost) return NotFound("Blog not found");
            if (dto.ParentCommentId.HasValue)
            {
                var parentComment = await _context.Comments.AnyAsync(c => c.Id == dto.ParentCommentId);
                if (!parentComment) return BadRequest("ParentComment not Found!");
            }

            var newComment = new Comment
            {
                BlogPostId = dto.BlogPostId,
                UserId = userId,
                Content = dto.Content,
                ParentCommentId = dto.ParentCommentId
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok(newComment);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub"));

            var comment = await _context.Comments
            .Where(c => c.UserId == userId && c.Id == id)
            .FirstOrDefaultAsync();

            if (comment == null) return NotFound();

            comment.Content = dto.Content;

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("blog/{blogPostId}")]
        public async Task<IActionResult> GetForBlog(Guid blogPostId, [FromQuery] PaginationQuery query)
        {
            var baseQuery = _context.Comments
            .Where(cm => cm.BlogPostId == blogPostId)
            .Include(c => c.User)
            .OrderBy(c => c.CreatedAt);

            var totalCount = await baseQuery.CountAsync();

            var comments = await baseQuery
            .Skip(query.Skip)
            .Take(query.PageSize)
            .Select(c=>new CommentResponseDto
            {
                Id = c.Id,
                Content = c.Content,
                AuthorUsername = c.User.Username,
                CreatedAt = c.CreatedAt,
                ParentCommentId = c.ParentCommentId
            })
            .ToListAsync();

            var result= new PagedResult<CommentResponseDto>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                Items = comments
            };

            return Ok(result);
        }
    }
}
