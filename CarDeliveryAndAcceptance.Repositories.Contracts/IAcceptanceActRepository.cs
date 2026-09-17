using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="AcceptanceAct"/>>
/// </summary>
public interface IAcceptanceActRepository
{   
    /// <summary>
    /// 
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceAct>> GetAcceptanceActsAsync(CancellationToken cancellationToken);
    
    
    Task<AcceptanceAct> GetAcceptanceActByIdAsync(Guid id , CancellationToken cancellationToken);
}