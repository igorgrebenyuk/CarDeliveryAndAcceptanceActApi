using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="City"/>
/// </summary>
public interface ICityRepository
{
    /// <summary>
    /// Получение всех городов
    /// </summary>
    Task<IReadOnlyCollection<City>> GetCitiesAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение города по ID
    /// </summary>
    Task<City?> GetCityByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск города по названию
    /// </summary>
    Task<City?> GetCityByNameAsync(string Name, CancellationToken cancellationToken);
}