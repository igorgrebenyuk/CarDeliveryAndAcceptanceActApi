using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface IAcceptanceActService
{
    /// <summary>
    /// Получение всех актов
    /// </summary>
    Task<IReadOnlyCollection<AcceptanceActListItemModel>> GetAcceptanceActsAsync(CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Получение акта по ID
    /// </summary>
    Task<AcceptanceActModel> GetAcceptanceActByIdAsync(Guid id , CancellationToken cancellationToken);
    
    /// <summary>
    ///Добавление акта
    /// </summary>
    Task<AcceptanceActModel> CreateAcceptanceActAsync(AcceptanceActCreateModel acceptanceActCreateModel, CancellationToken cancellationToken);

    /// <summary>
    ///Обновление акта
    /// </summary>
    Task UpdateAcceptanceActAsync(AcceptanceActUpdateModel acceptanceActModel, CancellationToken cancellationToken);

    /// <summary>
    ///Удаление акта
    /// </summary>
    Task DeleteAcceptanceActAsync(Guid id, CancellationToken cancellationToken);
}