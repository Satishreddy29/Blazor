using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace CustomerAPI.Tests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DbContextOptions<CustomerContext> _options;

        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _options = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;

            using (var context = new CustomerContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task GetCustomer_ExistingId_ReturnsOk()
        {
            // Arrange
            using (var context = new CustomerContext(_options))
            {
                var customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    Phone = "555-1234"
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/api/customer/1"); // Assuming ID starts at 1

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingId_ReturnsDeletedCustomer()
        {
            // Arrange
            using (var context = new CustomerContext(_options))
            {
                var customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    Phone = "555-1234"
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.DeleteAsync($"/api/customer/1"); // Assuming ID starts at 1

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutCustomer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            int customerId;
            using (var context = new CustomerContext(_options))
            {
                var customer = new Customer
                {
                    Name = "John Doe",
                    Address = "123 Main St",
                    Phone = "555-1234"
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
                customerId = customer.Id; // Capture the ID of the created customer
            }

            var updatedCustomer = new Customer
            {
                Id = customerId, // Use the captured ID
                Name = "Jane Doe",
                Address = "456 Elm St",
                Phone = "555-5678"
            };

            // Act
            var content = new StringContent(JsonSerializer.Serialize(updatedCustomer), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/customer/{customerId}", content); // Use the captured ID

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task PostCustomer_ValidCustomer_ReturnsCreated()
        {
            // Arrange
            var newCustomer = new Customer
            {
                Name = "Alice Smith",
                Address = "789 Oak St",
                Phone = "555-8765"
            };

            // Act
            var content = new StringContent(JsonSerializer.Serialize(newCustomer), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/customer", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
