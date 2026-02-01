using System.Security.Claims;
using BlogApp.Api.Data;
using BlogApp.Api.DTOs.Blog;
using BlogApp.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Route("api/blog-posts")]
    [ApiController]
    public class BlogPostsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [Authorize]
        [HttpPost]
        public IActionResult CreateBlogPost([FromForm] CreateBlogPostDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub"));

            var blogPost = new BlogPost
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = userId
            };

            _context.BlogPosts.Add(blogPost);
            _context.SaveChanges();

            return Ok(blogPost);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(Guid id, [FromForm] CreateBlogPostDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub"));

            var blog = await _context.BlogPosts
            .Where(bp => bp.Id == id)
            .FirstOrDefaultAsync();

            if (blog == null) return NotFound();
            if (blog.AuthorId != userId) return Forbid();

            blog.Title = dto.Title;
            blog.Content = dto.Content;
            _context.BlogPosts.Update(blog);
            _context.SaveChanges();

            return Ok(blog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub"));
            var blogPost = await _context.BlogPosts
            .Include(bp => bp.Author)
            .Where(bp => bp.Id == id && bp.AuthorId == userId)
            .FirstOrDefaultAsync();

            if (blogPost == null) return NotFound();

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Deleted successfully!",
                id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogPosts = await _context.BlogPosts
            .Include(bp => bp.Author)
            .OrderByDescending(bp => bp.CreatedAt)
            .Select(bp => new BlogPostResponseDto
            {
                Id = bp.Id,
                Title = bp.Title,
                Content = bp.Content,
                AuthorUsername = bp.Author.Username,
                CreatedAt = bp.CreatedAt
            })
            .ToListAsync();

            return Ok(blogPosts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var blogPost = await _context.BlogPosts
            .Include(bp => bp.Author)
            .Where(bp => bp.Id == id)
            .Select(bp => new BlogPostResponseDto
            {
                Id = bp.Id,
                Title = bp.Title,
                Content = bp.Content,
                AuthorUsername = bp.Author.Username,
                CreatedAt = bp.CreatedAt
            })
            .FirstOrDefaultAsync();

            if (blogPost == null)
                return NotFound();

            return Ok(blogPost);
        }
    }
}
