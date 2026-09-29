using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="Salesman"/>
/// </summary>
public class SalesmanRepository : BaseWriteRepository<Salesman>, ISalesmanRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public SalesmanRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех продавцов
    /// </summary>
    Task<IReadOnlyCollection<Salesman>> ISalesmanRepository.GetSalesmanAsync(CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<Salesman?> ISalesmanRepository.GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    ///<summary>
    ///Получения продавца по названию компании
    /// </summary>>
    Task<Salesman?> ISalesmanRepository.GetSalesmanByNameAsync( string Name, CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower() , cancellationToken);
}