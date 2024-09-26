using TrackingApi.Model;

namespace TrackingApi.Services
{
    public interface ICustomerOrderService
    {
        Task<Tracking> GetAllCustomerData(); // Change to return Tracking
        Task AddCustomerDataAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}