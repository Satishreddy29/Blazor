using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            List<Customer> custresult = (await httpClient.GetFromJsonAsync<IEnumerable<Customer>>("https://localhost:7023/api/customer")).ToList();
            List<Order> orderresult = (await httpClient.GetFromJsonAsync<IEnumerable<Order>>("https://localhost:7177/api/orders")).ToList();
            Tracking co = new Tracking();
            co.customers = custresult;
            co.orders = orderresult;
            return co;

        }

    }
   
}
