using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;


public class CityRepository : BaseWriteRepository<City>, ICityRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public CityRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех городов
    /// </summary>
    Task<IReadOnlyCollection<City>> ICityRepository.GetCitiesAsync(CancellationToken cancellationToken)
        => reader.Read<City>()
            .NotDeletedAt()
            .OrderBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение города по ID
    /// </summary>
    Task<City?> ICityRepository.GetCityByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<City>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// Поиск города по названию
    /// </summary>
    Task<City?> ICityRepository.GetCityByNameAsync(string cityName, CancellationToken cancellationToken)
        => reader.Read<City>()
            .NotDeletedAt()
            .Where(x => x.Name == cityName)
            .FirstOrDefaultAsync(cancellationToken);
}