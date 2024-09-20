using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "James", Address = "India", Phone = "6547893215"},
                new Customer { Id = 2, Name = "John", Address = "US", Phone = "6547893215" },
                new Customer { Id = 3, Name = "Noah", Address = "Austaria", Phone = "6547893215"},
                new Customer { Id = 4, Name = "Tim", Address = "India", Phone = "6547893215"},
                new Customer { Id = 5, Name = "Oliver", Address = "US", Phone = "6547893215"},
                new Customer { Id = 6, Name = "Theodore", Address = "India", Phone = "6547893215"},
                new Customer { Id = 7, Name = "Henry", Address = "US", Phone = "6547893215"},
                new Customer { Id = 8, Name = "Lucas", Address = "Germany", Phone = "6547893215"},
                new Customer { Id = 9, Name = "William", Address = "Austaria", Phone = "6547893215"},
                new Customer { Id = 10, Name = "Leo", Address = "India", Phone = "6547893215"},
                new Customer { Id = 11, Name = "Simran", Address = "US", Phone = "6547893215"},
                new Customer { Id = 12, Name = "Owen", Address = "India", Phone = "6547893215"},
                new Customer { Id = 13, Name = "Samuel", Address = "US", Phone = "6547893215"},
                new Customer { Id = 14, Name = "Levi", Address = "Germany", Phone = "6547893215"});
        }
    }
}
