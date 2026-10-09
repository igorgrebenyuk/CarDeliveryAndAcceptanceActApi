using CarDeliveryAndAcceptance.Services.Contracts.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;

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
    
    /// <summary>
    ///Добавление города
    /// </summary>
    Task CreateCityAsync(CityCreateModel cityCreateModel, CancellationToken cancellationToken);

    /// <summary>
    ///Обновление города
    /// </summary>
    Task UpdateCityAsync(CityModel cityModel, CancellationToken cancellationToken);

    /// <summary>
    ///Удаление города
    /// </summary>
    Task DeleteCityAsync(Guid id, CancellationToken cancellationToken);
}