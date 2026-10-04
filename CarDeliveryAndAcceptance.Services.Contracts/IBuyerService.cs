using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface IBuyerService
{
    /// <summary>
    /// Получение всех покупателей
    /// </summary>
    Task<IReadOnlyCollection<BuyerModel>> GetBuyersAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<BuyerModel?> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Поиск покупателя по названию компании
    /// </summary>
    Task<BuyerModel?> GetBuyerByNameAsync(string Name, CancellationToken cancellationToken);
}