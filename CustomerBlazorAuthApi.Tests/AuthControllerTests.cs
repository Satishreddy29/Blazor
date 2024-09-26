using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CustomerBlazorAuthApi.Controllers;
using CustomerBlazorAuthApi.Data;
using CustomerBlazorAuthApi.Models;
using Moq;

public class AuthControllerTests
{
    private readonly AuthController _authController;
    private readonly ApplicationDbContext _context;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<ILogger<AuthController>> _mockLogger;

    public AuthControllerTests()
    {
        // Set up the in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        
        // Seed the database with a valid user
        _context.Users.Add(new User { Username = "validuser", Password = "password" });
        _context.SaveChanges();

        _mockConfig = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<AuthController>>();

        _authController = new AuthController(_context, _mockConfig.Object, _mockLogger.Object);
    }

    [Fact]
    public void Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var user = new User { Username = "invaliduser", Password = "wrongpassword" };

        var result = _authController.Login(user) as UnauthorizedResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

}
