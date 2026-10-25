using Microsoft.EntityFrameworkCore;
using Dealership.Interfaces;
using Dealership.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class CarRepository : ICarRepository
{
    private readonly DealershipDbContext _dealershipDbContext;

    public CarRepository(DealershipDbContext dealershipDbContext)
    {
        _dealershipDbContext = dealershipDbContext;
    }

    public List<Car> GetAllCars()
    {
        return _dealershipDbContext.Cars.ToList();
    }

//https://www.linkedin.com/posts/rakesh-gupta-531a74100_csharp-dotnet-linq-share-7492405239732551680-AyAn/
    public Car? GetCarById(Guid id)
    {
        return _dealershipDbContext.Cars
            .Include(car => car.Owners)
            .FirstOrDefault(car => car.Id == id);
    }
    public Car? GetCarByName(string name)
    {
        return _dealershipDbContext.Cars
            .Include(car => car.Owners)
            .FirstOrDefault(car => car.Model == name);
    }

    public List<Car> GetWeeklySales()
    {
        //https://www.geeksforgeeks.org/c-sharp/datetime-adddays-method-in-c-sharp/
        DateTime weekAgo = DateTime.UtcNow.AddDays(-7);

        return _dealershipDbContext.Cars
            .Include(car => car.Owners)
            .Where(car => car.Sold && car.SaleDate >= weekAgo)
            .OrderByDescending(car => car.SaleDate)
            .ToList();
    }

    public void SaveChanges()
    {
        _dealershipDbContext.SaveChanges();
    }

}