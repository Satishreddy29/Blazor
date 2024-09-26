using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrackingApi.Controllers;
using TrackingApi.Model;
using TrackingApi.Services;
using Xunit;

namespace TrackingApi.Tests
{
    public class TrackingControllerTests
    {
        private readonly Mock<ICustomerOrderService> _mockService; // Use the interface here
        private readonly Mock<ILogger<TrackingController>> _mockLogger;
        private readonly TrackingController _controller;

        public TrackingControllerTests()
        {
            _mockService = new Mock<ICustomerOrderService>();
            _mockLogger = new Mock<ILogger<TrackingController>>();
            _controller = new TrackingController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCustomerOrders_ReturnsOkResult_WithCustomerOrders()
        {
            // Arrange
            var mockOrders = new Tracking { customers = new List<Customer> { new Customer() } };
            _mockService.Setup(service => service.GetAllCustomerData()).ReturnsAsync(mockOrders);

            // Act
            var result = await _controller.GetCustomerOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockOrders, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerOrders_ReturnsServerError_OnException()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllCustomerData()).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.GetCustomerOrders();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task PostCustomerOrders_ReturnsCreatedAtAction_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockService.Setup(service => service.AddCustomerDataAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostCustomerOrders(customer);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetCustomerOrders", createdResult.ActionName);
            Assert.Equal(customer, createdResult.Value);
        }

        [Fact]
        public async Task PostCustomerOrders_ReturnsServerError_OnException()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockService.Setup(service => service.AddCustomerDataAsync(customer)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.PostCustomerOrders(customer);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(customer, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result); // Expect NotFoundObjectResult
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Updated Customer" };
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync(customer);
            _mockService.Setup(service => service.UpdateCustomerAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCustomer(1, customer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Updated Customer" };
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.UpdateCustomer(1, customer);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Expect NotFoundObjectResult
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync(customer);
            _mockService.Setup(service => service.DeleteCustomerAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Expect NotFoundObjectResult
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
