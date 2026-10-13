using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;

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
    Task<BuyerModel> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Поиск покупателя по названию компании
    /// </summary>
    Task<BuyerModel> GetBuyerByNameAsync(string Name, CancellationToken cancellationToken);
    
    
    /// <summary>
    ///Добавление покупателя
    /// </summary>
    Task CreateBuyerAsync(BuyerCreateModel buyerCreateModel, CancellationToken cancellationToken);

    /// <summary>
    ///Обновление покупателя
    /// </summary>
    Task UpdateBuyerAsync(BuyerModel buyerModel, CancellationToken cancellationToken);

    /// <summary>
    ///Удаление покупателя
    /// </summary>
    Task DeleteBuyerAsync(Guid id, CancellationToken cancellationToken);
    
}