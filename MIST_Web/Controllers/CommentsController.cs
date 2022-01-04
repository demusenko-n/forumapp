using MIST.Context;
using MIST_infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MIST_infrastructure;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CommentsController(ApplicationContext context)
        {   
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Comment>>> Get()
        {
            return await _context.Comments.ToListAsync();
        }
        [HttpGet("clear")]
        public void Clear()
        {
            _context.Comments.RemoveRange(_context.Comments);
            _context.SaveChanges();
        }

        [HttpPost("add")]
        public async Task<TransferObj<Comment?>> Add(Comment comment)
        {
            try
            {
                comment.Date = DateTime.UtcNow;

                var resultsCheck = new List<ValidationResult>();
                if (!Validator.TryValidateObject(comment, new ValidationContext(comment), resultsCheck, true))
                    return new TransferObj<Comment?>(null, string.Join("\n", resultsCheck.Select(r => r.ErrorMessage)));

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return new TransferObj<Comment?>(comment);
            }
            catch (Exception ex)
            {
                return new TransferObj<Comment?>(null, ex.Message);
            }
        }
    }

}
