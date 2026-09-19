using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="Buyer"/>
/// </summary>
public interface IBuyerRepository
{
    /// <summary>
    /// Получение всех покупателей
    /// </summary>
    Task<IReadOnlyCollection<Buyer>> GetBuyersAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<Buyer?> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Поиск покупателя по названию компании
    /// </summary>
    Task<Buyer?> GetBuyerByNameAsync(string Name, CancellationToken cancellationToken);


}