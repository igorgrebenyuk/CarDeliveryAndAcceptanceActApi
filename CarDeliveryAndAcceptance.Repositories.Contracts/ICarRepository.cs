using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="Car"/>
/// </summary>
public interface ICarRepository
{
    /// <summary>
    /// Получение всех автомобилей
    /// </summary>
    Task<IReadOnlyCollection<Car>> GetCarsAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение автомобиля по ID
    /// </summary>
    Task<Car?> GetCarByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск автомобиля по VIN-коду
    /// </summary>
    Task<Car?> GetCarByVinAsync(string vinCode, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск автомобиля по регистрационному знаку
    /// </summary>
    Task<Car?> GetCarByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
}