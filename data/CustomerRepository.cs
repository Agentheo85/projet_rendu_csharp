using Microsoft.EntityFrameworkCore;
using Dealership.Interfaces;
using Dealership.Models;


public class CustomerRepository : ICustomerRepository
{
    private readonly DealershipDbContext _dealershipDbContext;

    public CustomerRepository(DealershipDbContext dealershipDbContext)
    {
        _dealershipDbContext = dealershipDbContext;
    }

    public List<Customer> GetAllCustomer()
    {
        return _dealershipDbContext.Customers.Include(customer=>customer.Cars).ToList(); // include sert a ajouter les class Cars sinon ef le fait aps tout seul
    }

    public Customer? GetCustomerById(Guid id)
    {
        return _dealershipDbContext.Customers
        .Include(customer => customer.Cars)
            .FirstOrDefault(customer => customer.Id == id);
    }
    public Customer? GetCustomerByFirstandLastName( string FirstName , string LastName)
    {
           return _dealershipDbContext.Customers
        .Include(customer => customer.Cars)
            .FirstOrDefault(customer => customer.FirstName == FirstName && customer.LastName == LastName);
    }

}