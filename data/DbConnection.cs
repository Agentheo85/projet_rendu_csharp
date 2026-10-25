using Npgsql;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class DbConnection
{
    private readonly DealershipDbContext dealershipDbContext;
    public DbConnection(DealershipDbContext dealershipDbContext)
    {
        this.dealershipDbContext = dealershipDbContext;
    }
    public void ClearDb()
    {
        
        dealershipDbContext.Cars.ExecuteDelete();
        dealershipDbContext.Customers.ExecuteDelete();
    }
    public void InitDb(List<Customer> customers, List<Car> cars)
    {
        // éviter la réimportation du csv 
        if (dealershipDbContext.Customers.Any()) return;

        dealershipDbContext.AddRange(customers);
        dealershipDbContext.AddRange(cars);
        dealershipDbContext.SaveChanges();
    }
}    