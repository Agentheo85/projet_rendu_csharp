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


    public Car? GetCarById(Guid id)
    {
        return _dealershipDbContext.Cars
            .FirstOrDefault(car => car.Id == id);
    }
    public Car? GetCarByName(string name)
    {
        return _dealershipDbContext.Cars.FirstOrDefault(car => car.Model == name);
    }

}