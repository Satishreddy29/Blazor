using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrdersAPI.Helpers;

namespace OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Order>> GetOrders()
        {
            try
            {
                return OrderJsonHelper.ReadFromJsonFile<Order>();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var people = OrderJsonHelper.ReadFromJsonFile<Order>();
            var Order = people.FirstOrDefault(p => p.Id == id);
            if (Order == null)
            {
                return NotFound();
            }
            return Order;
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder([FromBody] Order newOrder)
        {
            var people = OrderJsonHelper.ReadFromJsonFile<Order>();
            people.Add(newOrder);
            OrderJsonHelper.WriteToJsonFile(people);
            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            var people = OrderJsonHelper.ReadFromJsonFile<Order>();
            var Order = people.FirstOrDefault(p => p.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            Order.Name = updatedOrder.Name;
            Order.Email = updatedOrder.Email;
            Order.Phone = updatedOrder.Phone;
            OrderJsonHelper.WriteToJsonFile(people);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var people = OrderJsonHelper.ReadFromJsonFile<Order>();
            var Order = people.FirstOrDefault(p => p.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            people.Remove(Order);
            OrderJsonHelper.WriteToJsonFile(people);

            return NoContent();
        }
    }
}

