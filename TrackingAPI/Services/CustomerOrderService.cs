using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TrackingApi.Model;

namespace TrackingApi.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomerOrderService> _logger;

        public CustomerOrderService(HttpClient httpClient, IConfiguration configuration, ILogger<CustomerOrderService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        // Method to get all customer data with associated product tracking details
        public async Task<Tracking> GetAllCustomerData()
        {
            _logger.LogInformation("CustomerOrderService: Calling Method GetAllCustomerData");
            var customerApiUrl = _configuration["ApiSettings:CustomerApiUrl"];
            var orderApiUrl = _configuration["ApiSettings:OrderApiUrl"];

            try
            {
                var custResult = await _httpClient.GetFromJsonAsync<IEnumerable<Customer>>(customerApiUrl);
                var productResult = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(orderApiUrl);

                if (custResult == null || productResult == null)
                {
                    return new Tracking(); // Return empty if any data set is null
                }

                // Joining customer and product data
                var joinedData = from customer in custResult
                                 join product in productResult on customer.Id equals product.CustomerId into productGroup
                                 from product in productGroup.DefaultIfEmpty()
                                 select new Customer
                                 {
                                     Id = customer.Id,
                                     Name = customer.Name,
                                     Phone = customer.Phone,
                                     Address = customer.Address,
                                     ProductName = product?.Name ?? "---",
                                     TrackingId = product?.TrackingId ?? "---",
                                     TrackingStatus = product?.TrackingStatus ?? "No Order"
                                 };

                return new Tracking
                {
                    customers = joinedData.ToList(),
                    products = productResult.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllCustomerData: {ex.Message}");
                throw; // Rethrow exception for higher-level handling
            }
        }

        // Method to add new customer data
        public async Task AddCustomerDataAsync(Customer customer)
        {
            _logger.LogInformation("CustomerOrderService: Calling Method AddCustomerDataAsync, Customer Name: {Name}", customer.Name);
            var customerApiUrl = _configuration["ApiSettings:CustomerApiUrl"];

            try
            {
                var response = await _httpClient.PostAsJsonAsync(customerApiUrl, customer);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer: {ex.Message}");
                throw; // Handle error appropriately
            }
        }

        // Method to get customer by ID
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            _logger.LogInformation("CustomerOrderService: Fetching customer with ID: {Id}", id);
            var customerApiUrl = $"{_configuration["ApiSettings:CustomerApiUrl"]}/{id}";

            try
            {
                var customer = await _httpClient.GetFromJsonAsync<Customer>(customerApiUrl);
                return customer ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching customer by ID: {ex.Message}");
                throw; // Handle error appropriately
            }
        }

        // Method to update existing customer data
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _logger.LogInformation("CustomerOrderService: Updating customer with ID: {Id}", customer.Id);
            var customerApiUrl = $"{_configuration["ApiSettings:CustomerApiUrl"]}/{customer.Id}";

            try
            {
                var response = await _httpClient.PutAsJsonAsync(customerApiUrl, customer);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer: {ex.Message}");
                throw; // Handle error appropriately
            }
        }

        // Method to delete customer by ID
        public async Task DeleteCustomerAsync(int id)
        {
            _logger.LogInformation("CustomerOrderService: Deleting customer with ID: {Id}", id);
            var customerApiUrl = $"{_configuration["ApiSettings:CustomerApiUrl"]}/{id}";

            try
            {
                var response = await _httpClient.DeleteAsync(customerApiUrl);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting customer: {ex.Message}");
                throw; // Handle error appropriately
            }
        }
    }
}
