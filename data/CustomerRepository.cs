using Microsoft.EntityFrameworkCore;
using Dealership.Interfaces;
using Dealership.Models;

namespace CoursSupDeVinci;

public class CustomerRepository : ICustomerRepository
{
    private readonly DealershipDbContext _dealershipDbContext;

    public CustomerRepository(DealershipDbContext dealershipDbContext)
    {
        _dealershipDbContext = dealershipDbContext;
    }

    public List<Customer> GetAllCustomer()
    {
        return _dealershipDbContext.Customers.ToList();
    }

    public Customer? GetCustomerById(Guid id)
    {
        return _dealershipDbContext.Customers
            .FirstOrDefault(customer => customer.Id == id);
    }

}