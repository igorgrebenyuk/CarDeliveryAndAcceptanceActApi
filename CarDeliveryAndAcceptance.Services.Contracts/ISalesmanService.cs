using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Contracts;

public interface ISalesmanService
{
    /// <summary>
    /// Получение всех продавцов
    /// </summary>
    Task<IReadOnlyCollection<SalesmanModel>> GetSalesmanAsync(CancellationToken cancellationToken);


    /// <summary>
    /// Получение продавца по ID
    /// </summary>
    Task<SalesmanModel?> GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск продавца по Названию компании
    /// </summary>
    Task<SalesmanModel?> GetSalesmanByNameAsync(string Name, CancellationToken cancellationToken);
}