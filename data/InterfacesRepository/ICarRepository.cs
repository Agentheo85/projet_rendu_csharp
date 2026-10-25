using Dealership.Models;

namespace Dealership.Interfaces;

public interface ICarRepository
{
    // Récupérer toutes les voitures.
    List<Car> GetAllCars();

    // Récupérer une voiture à partir de son identifiant.
    Car? GetCarById(Guid id);
    Car? GetCarByName(string name);
    List<Car> GetWeeklySales();
    void SaveChanges();

}