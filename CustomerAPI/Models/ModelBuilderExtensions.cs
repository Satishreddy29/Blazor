using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "James Broadman", Address = "2347 University drive NY 2333", Phone = "983-672-9008"},
                new Customer { Id = 2, Name = "John Paul", Address = "8997 LongBeach blvd Tampa FL 78923", Phone = "654-78-3215" },
                new Customer { Id = 3, Name = "Noah Peterson", Address = "886 Washingtion square DC DC 10098", Phone = "443-553-2221"},
                new Customer { Id = 4, Name = "Tim Brookes", Address = "4322 Loundoun reserve Fairfax VA 20171", Phone = "890-334-5567"},
                new Customer { Id = 5, Name = "Oliver MCkinely", Address = "2341 Zulla Chase Princeton NJ 879116", Phone = "234-673-2241"});

        }
    }
}
