using CustomerAPI;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<CustomerContext>(options =>
        {
            options.UseInMemoryDatabase("CustomerDB");
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
