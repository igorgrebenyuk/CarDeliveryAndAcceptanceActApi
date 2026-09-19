using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="Car"/>
/// </summary>
public class CarRepository : BaseWriteRepository<Car>, ICarRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public CarRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех автомобилей
    /// </summary>
    Task<IReadOnlyCollection<Car>> ICarRepository.GetCarsAsync(CancellationToken cancellationToken)
        => reader.Read<Car>()
            .NotDeletedAt()
            .OrderBy(x => x.Mark)
            .ThenBy(x => x.Model)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение автомобиля по ID
    /// </summary>
    Task<Car?> ICarRepository.GetCarByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Car>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Поиск автомобиля по VIN-коду
    /// </summary>
    Task<Car?> ICarRepository.GetCarByVinAsync(string vinCode, CancellationToken cancellationToken)
        => reader.Read<Car>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.VinCode == vinCode , cancellationToken);

    /// <summary>
    /// Поиск автомобиля по регистрационному знаку
    /// </summary>
    Task<Car?> ICarRepository.GetCarByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
        => reader.Read<Car>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.LicensePlate == licensePlate , cancellationToken);
}