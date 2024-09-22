using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using CustomerBlazorServerApp.Data;

namespace CustomerBlazorServerApp.Services
{
    public class CustomerService
    {

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(HttpClient httpClient, IConfiguration configuration, ILogger<CustomerService> logger)
        {
            this.httpClient = httpClient;
            this.configuration = configuration; 
            _logger = logger;
        }

        public async Task<CustomerOrder> GetAllCustomerData()
        {
            _logger.LogInformation("CustomerService: Calling Method GetAllCustomerData");
            var trackingApiUrl = configuration["ApiSettings:TrackingApiUrl"];
            var custresult = await httpClient.GetFromJsonAsync<CustomerOrder>(trackingApiUrl);
            return custresult;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            _logger.LogInformation("CustomerService: Calling Method AddCustomerAsync,Customer Name : "+customer.Name);
            var trackingApiUrl = configuration["ApiSettings:TrackingApiUrl"];
            var response = await httpClient.PostAsJsonAsync(trackingApiUrl, customer);
            response.EnsureSuccessStatusCode();
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            //return await _httpClient.GetFromJsonAsync<Customer>($"api/customers/{id}");
            _logger.LogInformation("CustomerService: Calling Method GetCustomerById");
            var trackingApiUrl = configuration["ApiSettings:TrackingApiUrl"];
            var custresult = await httpClient.GetFromJsonAsync<Customer>(trackingApiUrl+"/"+id);
            return custresult;
        }
        public async Task UpdateCustomer(Customer customer)
        {
            //await _httpClient.PutAsJsonAsync($"api/customers/{customer.Id}", customer);
            _logger.LogInformation("CustomerService: Calling Method UpdateCustomer");
            var trackingApiUrl = configuration["ApiSettings:TrackingApiUrl"];
            await httpClient.PutAsJsonAsync<Customer>(trackingApiUrl+"/"+customer.Id,customer);
        }

        public async Task DeleteCustomer(int id)
{
    _logger.LogInformation("CustomerService: Calling Method DeleteCustomer with ID: {CustomerId}", id);
    
    var trackingApiUrl = configuration["ApiSettings:TrackingApiUrl"];
    
    try
    {
        var response = await httpClient.DeleteAsync($"{trackingApiUrl}/{id}");
        
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Customer with ID: {CustomerId} deleted successfully.", id);
        }
        else
        {
            _logger.LogWarning("Failed to delete customer with ID: {CustomerId}. Status Code: {StatusCode}", id, response.StatusCode);
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while deleting customer with ID: {CustomerId}", id);
    }
}

    }
   
}
