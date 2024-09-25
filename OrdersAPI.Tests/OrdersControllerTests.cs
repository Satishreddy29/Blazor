using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using OrdersAPI.Models;

namespace OrdersAPI.Tests
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private DbContextOptions<OrderContext> _options;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;

            using (var context = new OrderContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            _options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;
            // Arrange
            using (var context = new OrderContext(_options))
            {
                var product = new Product
                {
                    Sku = "SKU123",
                    Name = "Product Name",
                    Description = "Product Description",
                    TrackingId = "tracking-id",
                    TrackingStatus = "New",
                    CustomerId = 1,
                    Price = 10,
                    IsAvailable = true
                };
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProduct_ExistingId_ReturnsOk()
        {
            //_options = new DbContextOptionsBuilder<OrderContext>()
            //   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            //   .Options;
            // Arrange
            int productId;
            using (var context = new OrderContext(_options))
            {
                var product = new Product
                {
                    Sku = "SKU123",
                    Name = "Product Name",
                    Description = "Product Description",
                    TrackingId = "tracking-id",
                    TrackingStatus = "New",
                    CustomerId = 1,
                    Price = 10,
                    IsAvailable = true
                };
                context.Products.Add(product);
                await context.SaveChangesAsync();
                productId = product.Id; // Capture the ID for use in the test
            }

            // Act
            var response = await _client.GetAsync($"/api/orders/{productId}"); // Use the captured ID

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostProduct_ValidOrder_ReturnsCreated()
        {
            _options = new DbContextOptionsBuilder<OrderContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
               .Options;
            // Arrange
            var newProduct = new Product
            {
                Sku = "SKU123",
                Name = "New Product",
                Description = "New Product Description",
                TrackingId = "tracking-id",
                TrackingStatus = "New",
                CustomerId = 1,
                Price = 10,
                IsAvailable = true
            };

            // Act
            var content = new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/orders", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

       [Fact]
public async Task PutProduct_ExistingId_ReturnsNoContent()
{
            _options = new DbContextOptionsBuilder<OrderContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
               .Options;
            // Arrange
            int productId;
    using (var context = new OrderContext(_options))
    {
        var product = new Product
        {
            Sku = "SKU123",
            Name = "Product Name",
            Description = "Product Description",
            TrackingId = "tracking-id",
            TrackingStatus = "New",
            CustomerId = 1,
            Price = 10,
            IsAvailable = true
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        productId = product.Id; // Capture the ID for updating
    }

    var updatedProduct = new Product
    {
        Id = productId, // Use the captured ID
        Sku = "SKU456",
        Name = "Updated Product Name",
        Description = "Updated Description",
        TrackingId = "updated-tracking-id",
        TrackingStatus = "Updated",
        CustomerId = 1,
        Price = 15,
        IsAvailable = true
    };

    // Act
    var content = new StringContent(JsonSerializer.Serialize(updatedProduct), Encoding.UTF8, "application/json");
    var response = await _client.PutAsync($"/api/orders/{productId}", content);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
}

        [Fact]
        public async Task DeleteProduct_ExistingId_ReturnsOk()
        {
            _options = new DbContextOptionsBuilder<OrderContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
               .Options;
            // Arrange
            int productId;
            using (var context = new OrderContext(_options))
            {
                var product = new Product
                {
                    Sku = "SKU123",
                    Name = "Product Name",
                    Description = "Product Description",
                    TrackingId = "tracking-id",
                    TrackingStatus = "New",
                    CustomerId = 1,
                    Price = 10,
                    IsAvailable = true
                };
                context.Products.Add(product);
                await context.SaveChangesAsync();
                productId = product.Id; // Capture the ID for deletion
            }

            // Act
            var response = await _client.DeleteAsync($"/api/orders/{productId}"); // Use the captured ID

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

       
    }
}
