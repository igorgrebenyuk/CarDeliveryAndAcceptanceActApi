using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="AcceptanceAct"/>>
/// </summary>
public interface IAcceptanceActRepository
{   
    /// <summary>
    /// Получение всех актов
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceAct>> GetAcceptanceActsAsync(CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Получение акта по ID
    /// </summary>
    Task<AcceptanceAct?> GetAcceptanceActByIdAsync(Guid id , CancellationToken cancellationToken);
    
    
}