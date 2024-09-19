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

        public CustomerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<CustomerOrder> GetAllCustomerData()
        {
            var custresult = await httpClient.GetFromJsonAsync<CustomerOrder>("https://localhost:7190/api/Tracking");
            return custresult;

        }

    }
   
}
