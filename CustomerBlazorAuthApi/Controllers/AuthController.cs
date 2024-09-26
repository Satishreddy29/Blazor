// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerBlazorAuthApi.Models;
using CustomerBlazorAuthApi.Data;

namespace CustomerBlazorAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationDbContext context, IConfiguration configuration,ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation("AuthController: Calling Method Login,Username : "+user.Username);
            // Check if user is null
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            // Check for null or empty username/password
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Username and password cannot be empty.");
            }

            var existingUser = _context.Users.SingleOrDefault(u => u.Username == user.Username);
            if (existingUser == null || existingUser.Password != user.Password) // Ensure to use proper hashing in production
            {
                return Unauthorized();
            }
            System.IdentityModel.Tokens.Jwt.JwtSecurityToken token;
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, existingUser.Username)
                };
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

             }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token.");
                return StatusCode(500, "Internal server error.");
            }

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            _logger.LogInformation("AuthController: Calling Method Register,Username : "+user.Username);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}
