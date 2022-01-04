using MIST.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MIST_infrastructure.Entities;
using MIST_infrastructure;


namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PostsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.CommentAuthor)
                .OrderByDescending(post => post.Comments.Any() ? post.Comments.Max(comment => comment.Date) : post.Date)
                .ToListAsync();
        }

        [HttpGet("clear")]
        public void Clear()
        {
            _context.Posts.RemoveRange(_context.Posts);
            _context.SaveChanges();
        }

        [HttpPost("add")]
        public async Task<TransferObj<Post?>> Add(Post post)
        {
            try
            {
                post.Date = DateTime.UtcNow;

                var resultsCheck = new List<ValidationResult>();
                if (!Validator.TryValidateObject(post, new ValidationContext(post), resultsCheck, true))
                    return new TransferObj<Post?>(null, string.Join("\n", resultsCheck.Select(r => r.ErrorMessage)));

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return new TransferObj<Post?>(post);
            }
            catch (Exception ex)
            {
                return new TransferObj<Post?>(null, ex.Message);
            }
        }
    }

}
