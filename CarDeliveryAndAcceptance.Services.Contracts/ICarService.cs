using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface ICarService
{
    /// <summary>
    /// Получение всех автомобилей
    /// </summary>
    Task<IReadOnlyCollection<CarModel>> GetCarsAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение автомобиля по ID
    /// </summary>
    Task<CarModel?> GetCarByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск автомобиля по VIN-коду
    /// </summary>
    Task<CarModel?> GetCarByVinCodeAsync(string vinCode, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск автомобиля по регистрационному знаку
    /// </summary>
    Task<CarModel?> GetCarByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
}