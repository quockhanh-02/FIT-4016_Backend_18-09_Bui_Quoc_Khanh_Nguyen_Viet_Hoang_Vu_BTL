using Microsoft.AspNetCore.Mvc;
using FleetManagement.Data;
using FleetManagement.Models;
using System.Linq;

namespace FleetManagement.Controllers
{
    [Route("api/[controller]")] 
    // Route: api/Auth

    [ApiController] 
    // Đánh dấu API controller
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; 
        // DbContext để truy vấn database

        public AuthController(AppDbContext context)
        {
            _context = context; 
            // Inject DbContext
        }

        [HttpPost("login")] 
        // POST: api/Auth/login
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var user = _context.Users
                .FirstOrDefault(u =>
                    u.Username == login.Username &&
                    u.Password == login.Password); 
                // Tìm user theo username + password

            if (user == null)
                return Unauthorized("Wrong username or password"); 
                // Sai thông tin → 401

            return Ok(user); 
            // Trả về user nếu đúng
        }
    }
}