using Microsoft.EntityFrameworkCore;
using Dealership.Interfaces;
using Dealership.Models;
namespace CoursSupDeVinci;

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

}