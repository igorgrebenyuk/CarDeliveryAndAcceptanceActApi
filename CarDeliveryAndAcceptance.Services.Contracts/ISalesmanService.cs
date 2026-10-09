using CarDeliveryAndAcceptance.Services.Contracts.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

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
    Task<SalesmanModel> GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Поиск продавца по Названию компании
    /// </summary>
    Task<SalesmanModel> GetSalesmanByNameAsync(string Name, CancellationToken cancellationToken);
    
    /// <summary>
    ///Добавление продавца
    /// </summary>
    Task CreateSalesmanAsync(SalesmanCreateModel salesmanCreateModel, CancellationToken cancellationToken);

    /// <summary>
    ///Обновление продавца
    /// </summary>
    Task UpdateSalesmanAsync(SalesmanModel salesmanModel, CancellationToken cancellationToken);

    /// <summary>
    ///Удаление продавца
    /// </summary>
    Task DeleteSalesmanAsync(Guid id, CancellationToken cancellationToken);
}