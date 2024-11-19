using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface ICustomerRepository
{
    Task<Customer> GetCustomerByPhoneNumber(string mobile);
    Task<int> GetCustomerOrdersCount(CustomerCountDTO customerDTO);
    Task<int> AddCustomer(Customer customer);
}
