using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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
    }
}
