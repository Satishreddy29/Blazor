using Microsoft.AspNetCore.Mvc;
using TrackingApi.Model;
using TrackingApi.Services;

namespace TrackingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly CustomerOrderService _customerOrderService;
        private readonly ILogger<TrackingController> _logger;

        public TrackingController(CustomerOrderService customerOrderService, ILogger<TrackingController> logger)
        {
            _customerOrderService = customerOrderService;
            _logger = logger;
        }

        // GET: api/tracking
        [HttpGet]
        public async Task<ActionResult<Tracking>> GetCustomerOrders()
        {
            _logger.LogInformation("TrackingController: Calling Method GetCustomerOrders");
            try
            {
                var result = await _customerOrderService.GetAllCustomerData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TrackingController: Error in Method GetCustomerOrders - {ex.Message}");
                return StatusCode(500, "An error occurred while fetching customer orders.");
            }
        }

        // POST: api/tracking
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomerOrders(Customer customer)
        {
            _logger.LogInformation($"TrackingController: Calling Method PostCustomerOrders, Customer Name: {customer.Name}");
            try
            {
                await _customerOrderService.AddCustomerDataAsync(customer);
                return CreatedAtAction(nameof(GetCustomerOrders), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TrackingController: Error in Method PostCustomerOrders - {ex.Message}");
                return StatusCode(500, "An error occurred while adding the customer.");
            }
        }

        // GET: api/tracking/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            _logger.LogInformation($"TrackingController: Fetching customer by ID: {id}");
            try
            {
                var customer = await _customerOrderService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound($"Customer with ID {id} not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TrackingController: Error fetching customer by ID - {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the customer.");
            }
        }

        // PUT: api/tracking/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                _logger.LogError("TrackingController: Customer ID mismatch.");
                return BadRequest("Customer ID mismatch.");
            }

            try
            {
                var existingCustomer = await _customerOrderService.GetCustomerByIdAsync(id);
                if (existingCustomer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound($"Customer with ID {id} not found.");
                }

                _logger.LogInformation($"TrackingController: Updating customer with ID: {id}");
                await _customerOrderService.UpdateCustomerAsync(customer);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"TrackingController: Error updating customer - {ex.Message}");
                return StatusCode(500, "An error occurred while updating the customer.");
            }
        }

        // DELETE: api/tracking/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            _logger.LogInformation($"TrackingController: Deleting customer with ID: {id}");
            try
            {
                var customer = await _customerOrderService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound($"Customer with ID {id} not found.");
                }

                await _customerOrderService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"TrackingController: Error deleting customer - {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the customer.");
            }
        }
    }
}
