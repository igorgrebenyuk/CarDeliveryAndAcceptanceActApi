using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Contracts;

/// <summary>
/// Репозиторий работы с <see cref="Salesman"/>
/// </summary>
public interface ISalesmanRepository
{
    /// <summary>
    /// Получение всех продавцов
    /// </summary>
    Task<IReadOnlyCollection<Salesman>> GetSalesmanAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение продавца по ID
    /// </summary>
    Task<Salesman?> GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск продавца по Названию компании
    /// </summary>
    Task<Salesman?> GetSalesmanByNameAsync(string Name, CancellationToken cancellationToken);

}