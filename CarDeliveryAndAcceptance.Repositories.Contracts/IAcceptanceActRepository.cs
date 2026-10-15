using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="AcceptanceAct"/>>
/// </summary>
public interface IAcceptanceActRepository: IBaseWriteRepository<AcceptanceAct>
{   
    /// <summary>
    /// Получение всех актов
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceAct>> GetAcceptanceActsAsync(CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Получение акта по ID
    /// </summary>
    Task<AcceptanceAct?> GetAcceptanceActByIdAsync(Guid id , CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Поиск акта по сочетанию автомобиля, покупателя и продавца
    /// </summary>
    Task<AcceptanceAct?> GetAcceptanceActByParticipantsAsync(Guid carId, Guid buyerId, Guid salesmanId, CancellationToken cancellationToken);
    
    
}