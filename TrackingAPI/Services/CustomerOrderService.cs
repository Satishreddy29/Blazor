using TrackingApi.Model;

namespace TrackingApi.Services
{
    public class CustomerOrderService
    {

        private readonly HttpClient httpClient;

        public CustomerOrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Tracking> GetAllCustomerData()
        {
            // Fetch customer data
            var custresult = await httpClient.GetFromJsonAsync<IEnumerable<Customer>>("https://localhost:7023/api/customer");
            var productresult = await httpClient.GetFromJsonAsync<IEnumerable<Product>>("https://localhost:7177/api/orders");

            // Ensure the API calls returned results
            if (custresult == null || productresult == null)
            {
                return new Tracking(); // Or throw an exception
            }

            // Fetch tracking statuses asynchronously
            
           
            // Join customer data with tracking statuses
            var joinedData = from customer in custresult
                            join status in productresult on customer.Id equals status.CustomerId into statusGroup
                            from status in statusGroup.DefaultIfEmpty() // Left join to include customers without statuses
                            select new Customer
                            {
                                Id = customer.Id,
                                Name = customer.Name, // Assuming Customer has a Name property
                                Phone = customer.Phone, // Assuming Customer has a Name property
                                Address = customer.Address, // Assuming Customer has a Name property
                                ProductName = string.IsNullOrEmpty(status?.Name) ? "---" : status.Name,
                                TrackingId = string.IsNullOrEmpty(status?.TrackingId) ? "---" : status.TrackingId,
                                TrackingStatus = string.IsNullOrEmpty(status?.TrackingStatus) ? "No Order" : status.TrackingStatus
                            };

            // Create and return the tracking object
            return new Tracking
            {
                customers = joinedData.ToList(),
                products = productresult.ToList()
            };
        }

        public async Task AddCustomerDataAsync(Customer customer)
        {
            var response = await httpClient.PostAsJsonAsync("https://localhost:7023/api/customer", customer);
            response.EnsureSuccessStatusCode(); // Check for successful response
        }

    }
   
}
