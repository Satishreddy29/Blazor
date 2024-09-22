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

    }
   
}
