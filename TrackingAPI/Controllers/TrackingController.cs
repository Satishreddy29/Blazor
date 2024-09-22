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
        private readonly ILogger<TrackingController> _logger;
        
        public TrackingController(CustomerOrderService customerOrderService,ILogger<TrackingController> logger) 
        {
            _customerOrderService = customerOrderService;
            _logger = logger;
            
        }
        [HttpGet]
        public ActionResult<Tracking> GetCustomerOrders()
        {
            _logger.LogInformation("TrackingController: Calling Method GetCustomerOrders");
            try
            {
                return _customerOrderService.GetAllCustomerData().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError("TrackingController: Error in Method GetCustomerOrders,"+ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomerOrders(Customer customer)
        {
            _logger.LogInformation("TrackingController: Calling Method PostCustomerOrders,Customer Id : "+customer.Id);
            try
            {
                await _customerOrderService.AddCustomerDataAsync(customer);
                return CreatedAtAction(nameof(GetCustomerOrders), new { id = customer.Id }, customer); 
            }
            catch (Exception ex)
            {
                _logger.LogError("TrackingController: Error in Method PostCustomerOrders,"+ex.Message);
                return StatusCode(500, ex.Message); 
            }
        }
    }
}
