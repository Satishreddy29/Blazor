
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(CustomerContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCustomers()
        {
            _logger.LogInformation("CustomerController: Calling Method GetAllCustomers");
            return Ok(await _context.Customers.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(int id)
        {
            _logger.LogInformation("Calling Method GetCustomer, Customer Id :"+id);
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }
            return Ok(Customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer Customer)
        {
            _logger.LogInformation("CustomerController: Calling Method PostCustomer,Customer Name : "+Customer.Name);
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetCustomer",
                new { id = Customer.Id },
                Customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomer(int id, Customer Customer)
        {
            _logger.LogInformation("CustomerController: Calling Method PutCustomer,Customer Name : "+Customer.Name);
            if (id != Customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(Customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(p => p.Id == id))
                {
                    return NotFound();
                } 
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            _logger.LogInformation("CustomerController: Calling Method DeleteCustomer,Customer Id : "+id);
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(Customer);
            await _context.SaveChangesAsync();

            return Customer;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery]int[] ids)
        {
            _logger.LogInformation("CustomerController: Calling Method DeleteMultiple,Customer Ids : "+string.Join<int>(",", ids));
            var Customers = new List<Customer>();
            foreach (var id in ids)
            {
                var Customer = await _context.Customers.FindAsync(id);

                if (Customer == null)
                {
                    return NotFound();
                }

                Customers.Add(Customer);
            }

            _context.Customers.RemoveRange(Customers);
            await _context.SaveChangesAsync();

            return Ok(Customers);
        }
    }
}