using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;

/// <summary>
/// Репозиторий работы с <see cref="AcceptanceAct"/>
/// </summary>
public class AcceptanceActRepository : BaseWriteRepository<AcceptanceAct>, IAcceptanceActRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public AcceptanceActRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <inheritdoc />
    Task<IReadOnlyCollection<AcceptanceAct>> IAcceptanceActRepository.GetAcceptanceActsAsync(CancellationToken cancellationToken)
        => reader.Read<AcceptanceAct>()
            .NotDeletedAt()
            .Include(x => x.City)
            .Include(x => x.Buyer)
            .Include(x => x.Salesman)
            .Include(x => x.Car)
            .OrderBy(x => x.CreatedAt)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <inheritdoc />
    Task<AcceptanceAct?> IAcceptanceActRepository.GetAcceptanceActByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AcceptanceAct>()
            .NotDeletedAt()
            .Include(x => x.City)
            .Include(x => x.Buyer)
            .Include(x => x.Salesman)
            .Include(x => x.Car)
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);
    
    Task<AcceptanceAct?> IAcceptanceActRepository.GetAcceptanceActByParticipantsAsync(Guid carId, Guid buyerId, Guid salesmanId, CancellationToken cancellationToken)
        => reader.Read<AcceptanceAct>()
            .NotDeletedAt()
            .FirstOrDefaultAsync(x => x.CarId == carId 
                                      && x.BuyerId == buyerId 
                                      && x.SalesmanId == salesmanId,
                                        cancellationToken);
}