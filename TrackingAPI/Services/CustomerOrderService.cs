using TrackingApi.Model;
using Microsoft.Extensions.Configuration;

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
        public async Task<Tracking> GetAllCustomerData()
        {
            _logger.LogInformation("CustomerOrderService: Calling Method GetAllCustomerData");
            var customerApiUrl = configuration["ApiSettings:CustomerApiUrl"];
            var orderApiUrl = configuration["ApiSettings:OrderApiUrl"];
            
            var custresult = await httpClient.GetFromJsonAsync<IEnumerable<Customer>>(customerApiUrl);
            var productresult = await httpClient.GetFromJsonAsync<IEnumerable<Product>>(orderApiUrl);

            if (custresult == null || productresult == null)
            {
                return new Tracking(); 
            }

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

        public async Task AddCustomerDataAsync(Customer customer)
        {
            _logger.LogInformation("CustomerOrderService: Calling Method AddCustomerDataAsync, Customer Name : "+ customer.Name);
            var customerApiUrl = configuration["ApiSettings:CustomerApiUrl"];
            var response = await httpClient.PostAsJsonAsync(customerApiUrl, customer);
            response.EnsureSuccessStatusCode();
        }

    }
   
}
