using Dealership.Data;
using Npgsql;

namespace CoursSupDeVinci;

public class DbConnection
{
    private readonly DealershipDbContext dealershipDbContext;
    public DbConnection(DealershipDbContext dealershipDbContext)
    {
        this.dealershipDbContext = dealershipDbContext;
    }
// les clients et les voitures lus dans les CSV, puis enregistrement
    public void initDb(List<Customer> customers, List<Car> cars)
    {
        // éviter la réimportation du csv 
        if (dealershipDbContext.Customers.Any()) return;

        dealershipDbContext.AddRange(customers);
        dealershipDbContext.AddRange(cars);
        dealershipDbContext.SaveChanges();
    }
}    