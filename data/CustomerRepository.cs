using CoursSupDeVinci.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace CoursSupDeVinci;

public class CustomerRepository : ICustomerRepository
{
    private readonly DealershipDbContext dealershipDbContext;

    public PersonRepository(DealershipDbContext dealershipDbContext)
    {
        _dealershipDbContext = dealershipDbContext;
    }

    public List<Customer> GetAllCustomer()
    {
        return _dealershipDbContext.Customer.ToList();
    }

    public Customer? GetCustomerById(Guid id)
    {
        return _dealershipDbContext.Customers
            .FirstOrDefault(customer => customer.Id == id);
    }

}