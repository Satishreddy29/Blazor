using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        Task<ActionResult<IEnumerable<Customer>>> GetCustomerById(int id);
        Task<ActionResult> InsertCustomerDeatils(Customer customer);
       // Task<ActionResult<Customer>> DeleteCustomerById(int id);
    }
}
