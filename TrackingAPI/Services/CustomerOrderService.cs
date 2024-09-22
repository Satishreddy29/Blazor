using TrackingApi.Model;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TrackingApi.Services
{
    public class CustomerOrderService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<CustomerOrderService> _logger;

        public CustomerOrderService(HttpClient httpClient, IConfiguration configuration, ILogger<CustomerOrderService> logger)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            _logger = logger;
        }

        // Method to get all customer data with associated product tracking details
        public async Task<Tracking> GetAllCustomerData()
        {
            _logger.LogInformation("CustomerOrderService: Calling Method GetAllCustomerData");
            var customerApiUrl = configuration["ApiSettings:CustomerApiUrl"];
            var orderApiUrl = configuration["ApiSettings:OrderApiUrl"];

            try
            {
                var custresult = await httpClient.GetFromJsonAsync<IEnumerable<Customer>>(customerApiUrl);
                var productresult = await httpClient.GetFromJsonAsync<IEnumerable<Product>>(orderApiUrl);

                if (custresult == null || productresult == null)
                {
                    return new Tracking(); // Return empty if any data set is null
                }

                // Joining customer and product data
                var joinedData = from customer in custresult
                                 join status in productresult on customer.Id equals status.CustomerId into statusGroup
                                 from status in statusGroup.DefaultIfEmpty()
                                 select new Customer
                                 {
                                     Id = customer.Id,
                                     Name = customer.Name,
                                     Phone = customer.Phone,
                                     Address = customer.Address,
                                     ProductName = string.IsNullOrEmpty(status?.Name) ? "---" : status.Name,
                                     TrackingId = string.IsNullOrEmpty(status?.TrackingId) ? "---" : status.TrackingId,
                                     TrackingStatus = string.IsNullOrEmpty(status?.TrackingStatus) ? "No Order" : status.TrackingStatus
                                 };

                return new Tracking
                {
                    customers = joinedData.ToList(),
                    products = productresult.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllCustomerData: {ex.Message}");
                throw; // Optionally, rethrow the exception or handle it gracefully
            }
        }

        // Method to add new customer data
        public async Task AddCustomerDataAsync(Customer customer)
        {
            _logger.LogInformation("CustomerOrderService: Calling Method AddCustomerDataAsync, Customer Name: " + customer.Name);
            var customerApiUrl = configuration["ApiSettings:CustomerApiUrl"];

            try
            {
                var response = await httpClient.PostAsJsonAsync(customerApiUrl, customer);
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
            _logger.LogInformation($"CustomerOrderService: Fetching customer with ID: {id}");
            var customerApiUrl = $"{configuration["ApiSettings:CustomerApiUrl"]}/{id}";

            try
            {
                var customer = await httpClient.GetFromJsonAsync<Customer>(customerApiUrl);
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
            _logger.LogInformation($"CustomerOrderService: Updating customer with ID: {customer.Id}");
            var customerApiUrl = $"{configuration["ApiSettings:CustomerApiUrl"]}/{customer.Id}";

            try
            {
                var response = await httpClient.PutAsJsonAsync(customerApiUrl, customer);
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
            _logger.LogInformation($"CustomerOrderService: Deleting customer with ID: {id}");
            var customerApiUrl = $"{configuration["ApiSettings:CustomerApiUrl"]}/{id}";

            try
            {
                var response = await httpClient.DeleteAsync(customerApiUrl);
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
