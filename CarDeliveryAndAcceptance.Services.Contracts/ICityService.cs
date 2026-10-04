using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface ICityService
{
    /// <summary>
    /// Получение всех городов
    /// </summary>
    Task<IReadOnlyCollection<CityModel>> GetCitiesAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение города по ID
    /// </summary>
    Task<CityModel?> GetCityByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск города по названию
    /// </summary>
    Task<CityModel?> GetCityByNameAsync(string Name, CancellationToken cancellationToken);
}