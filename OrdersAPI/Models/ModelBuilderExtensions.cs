using Microsoft.EntityFrameworkCore;

namespace OrdersAPI.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Active Wear - Men" },
                new Category { Id = 2, Name = "Active Wear - Women" },
                new Category { Id = 3, Name = "Mineral Water" },
                new Category { Id = 4, Name = "Publications" },
                new Category { Id = 5, Name = "Supplements" });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = 1, Name = "IPhone - 16 ", Sku = "AWMGSJ", Price = 1000, IsAvailable = true,TrackingId="12345",TrackingStatus="Ordered",CustomerId=1 },
                new Product { Id = 2, CategoryId = 1, Name = "Polo Shirt", Sku = "AWMPS", Price = 35, IsAvailable = true ,TrackingId="43123",TrackingStatus="Shipped",CustomerId=2},
                new Product { Id = 3, CategoryId = 1, Name = "Skater Graphic T-Shirt", Sku = "AWMSGT", Price = 33, IsAvailable = true ,TrackingId="345345",TrackingStatus="Shipped",CustomerId=3},
                new Product { Id = 4, CategoryId = 1, Name = "Slicker Jacket", Sku = "AWMSJ", Price = 125, IsAvailable = true ,TrackingId="234234",TrackingStatus="Shipped",CustomerId=4},
                new Product { Id = 5, CategoryId = 1, Name = "Thermal Fleece Jacket", Sku = "AWMTFJ", Price = 60, IsAvailable = true ,TrackingId="553",TrackingStatus="Delivered",CustomerId=5});
        }
    }
}
