using CoursSupDeVinci.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace CoursSupDeVinci;

public class CarRepository : ICarRepository
{
    private readonly DealershipDbContext dealershipDbContext;

    public CarRepository(DealershipDbContext dealershipDbContext)
    {
        _dealershipDbContext = dealershipDbContext;
    }

    public List<Car> GetAllCustomer()
    {
        return _dealershipDbContext.Car.ToList();
    }


    public Car? GetCarById(Guid id)
    {
        return _dealershipDbContext.Customers
            .FirstOrDefault(car => car.Id == id);
    }

}