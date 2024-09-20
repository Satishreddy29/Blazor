using Microsoft.AspNetCore.Mvc;
using TrackingApi.Model;
using TrackingApi.Services;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
         
    {
        private CustomerOrderService _customerOrderService;
        
        public TrackingController(CustomerOrderService customerOrderService) 
        {
            _customerOrderService = customerOrderService;
            
        }
        [HttpGet]
        public ActionResult<Tracking> GetCustomerOrders()
        {
            try
            {
                return _customerOrderService.GetAllCustomerData().Result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomerOrders(Customer customer)
        {
            try
            {
                await _customerOrderService.AddCustomerDataAsync(customer);
                return CreatedAtAction(nameof(GetCustomerOrders), new { id = customer.Id }, customer); // Return 201 Created
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }
    }
}
