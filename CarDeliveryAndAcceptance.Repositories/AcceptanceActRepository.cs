using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;

public class AcceptanceActRepository : IAcceptanceActRepository
{
    private readonly IReader reader;
    
    /// <summary>
    /// ctor.
    /// </summary>
    public AcceptanceActRepository(IReader reader)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех актов
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceAct>> IAcceptanceActRepository.GetAcceptanceActsAsync(CancellationToken cancellationToken)
    => reader.Read<AcceptanceAct>()
            .NotDeletedAt()
            .OrderBy(x => x.CarId)
            .ThenBy(x => x.BuyerId)
            .ThenBy(x => x.SalesmanId)
            .ToReadOnlyCollectionAsync(cancellationToken);
    

    Task <AcceptanceAct?> IAcceptanceActRepository.GetAcceptanceActByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<AcceptanceAct>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);
}