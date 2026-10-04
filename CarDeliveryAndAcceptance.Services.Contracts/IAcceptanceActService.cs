using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface IAcceptanceActService
{
    /// <summary>
    /// Получение всех актов
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceActModel>> GetAcceptanceActsAsync(CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Получение акта по ID
    /// </summary>
    Task<AcceptanceActModel?> GetAcceptanceActByIdAsync(Guid id , CancellationToken cancellationToken);
}