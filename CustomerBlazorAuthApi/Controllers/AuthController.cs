// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerBlazorAuthApi.Models;
using CustomerBlazorAuthApi.Data;

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
        if (user == null)
        return BadRequest(new { message = "User data is required" });

        var dbUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
        
        if (dbUser == null)
        {
            return Unauthorized(new { message = "Invalid credentials" }); // Ensure this is a JSON response
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, dbUser.Username)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

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
