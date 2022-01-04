using MIST.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MIST_infrastructure;
using MIST_infrastructure.Entities;
using System.Text.Json;
using System.Diagnostics;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public UsersController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return JsonSerializer.Serialize(await _context.Users.Include(x => x.Comments).Include(x => x.Posts).ToListAsync(), new JsonSerializerOptions() { ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles});
        }

        [HttpGet("clear")]
        public void Clear()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }
        [HttpPost("auth")]
        public async Task<TransferObj<User?>> Auth(string? login, string? password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
                string errMsg = "";
                if (user == null)
                    errMsg = "Invalid login or password";

                return new TransferObj<User?>(user, errMsg);
            }
            catch (Exception ex)
            {
                return new TransferObj<User?>(null, ex.Message);
            }
        }
        [HttpPost("reg")]
        public async Task<TransferObj<User?>> Reg(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

                if (existingUser != null)
                    return new TransferObj<User?>(null, "Login is already in use");

                Console.WriteLine("reached checks");

                var resultsCheck = new List<ValidationResult>();
                if (!Validator.TryValidateObject(user, new ValidationContext(user), resultsCheck, true))
                    return new TransferObj<User?>(null, string.Join("\n", resultsCheck.Select(r => r.ErrorMessage)));

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new TransferObj<User?>(user);
            }
            catch (Exception ex)
            {
                return new TransferObj<User?>(null, ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<TransferObj<User?>> Update(User user)
        {
            try
            {
                var resultsCheck = new List<ValidationResult>();
                if (!Validator.TryValidateObject(user, new ValidationContext(user), resultsCheck, true))
                    return new TransferObj<User?>(null, string.Join("\n", resultsCheck.Select(r => r.ErrorMessage)));


                //if login has changed but it is already in use
                var conflictingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Id != user.Id);


                if (conflictingUser != null)
                    return new TransferObj<User?>(null, "Login is already in use");


                _context.Update(user);
                await _context.SaveChangesAsync();

                return new TransferObj<User?>(user);
            }
            catch (Exception ex)
            {
                return new TransferObj<User?>(null, ex.Message);
            }
        }
    }
}
