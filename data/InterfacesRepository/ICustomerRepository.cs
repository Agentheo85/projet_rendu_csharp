using Dealership.Models;

namespace Dealership.Interfaces;

public interface ICustomerRepository
{
    List<Customer> GetAllCustomer();
    
    Customer ? GetCustomerById( Guid id);
    
    Customer ? GetCustomerByFirstandLastName( string FirstName , string LastName);

}