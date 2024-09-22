using CustomerAPI.Models;
using CustomerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Repositories.Concrete
{
    public class CustomerRepository
    {

        public async Task<List<Customer>> GetAllCustomers()
        {
            List<Customer> lstCustomers = new List<Customer>();

            return lstCustomers;

        }

        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {


            return null;
        }
        public async Task<ActionResult> InsertCustomerDeatils(Customer fcustomer)
        {


            return null;
        }
    }
}
