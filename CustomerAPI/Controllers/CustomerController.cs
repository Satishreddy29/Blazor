
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerAPI.Models;
using CustomerAPI.Helpers;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Customer>> GetCustomers()
        {
            try
            {
                return CustomerJsonHelper.ReadFromJsonFile<Customer>();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var people = CustomerJsonHelper.ReadFromJsonFile<Customer>();
            var Customer = people.FirstOrDefault(p => p.Id == id);
            if (Customer == null)
            {
                return NotFound();
            }
            return Customer;
        }

        [HttpPost]
        public ActionResult<Customer> CreateCustomer([FromBody] Customer newCustomer)
        {
            var people = CustomerJsonHelper.ReadFromJsonFile<Customer>();
            people.Add(newCustomer);
            CustomerJsonHelper.WriteToJsonFile(people);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var people = CustomerJsonHelper.ReadFromJsonFile<Customer>();
            var Customer = people.FirstOrDefault(p => p.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            Customer.Name = updatedCustomer.Name;
            Customer.Address = updatedCustomer.Address;
            Customer.Phone = updatedCustomer.Phone;
            CustomerJsonHelper.WriteToJsonFile(people);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            var people = CustomerJsonHelper.ReadFromJsonFile<Customer>();
            var Customer = people.FirstOrDefault(p => p.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            people.Remove(Customer);
            CustomerJsonHelper.WriteToJsonFile(people);

            return NoContent();
        }
    }
}
