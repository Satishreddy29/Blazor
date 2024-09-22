using Microsoft.EntityFrameworkCore;
using CustomerBlazorAuthApi.Models;

namespace CustomerBlazorAuthApi.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin" },
                new User { Id = 2, Username = "emp1", Password = "emp1" }
            );
        }
    }
}