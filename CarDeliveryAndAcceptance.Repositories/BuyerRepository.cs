using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="Buyer"/>
/// </summary>
public class BuyerRepository : BaseWriteRepository<Buyer>, IBuyerRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public BuyerRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех покупателей
    /// </summary>
    Task<IReadOnlyCollection<Buyer>> IBuyerRepository.GetBuyersAsync(CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<Buyer?> IBuyerRepository.GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    ///<summary>
    ///Получения покупателя по названию компании
    /// </summary>>
    Task<Buyer?> IBuyerRepository.GetBuyerByNameAsync( string Name, CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower() , cancellationToken);

}