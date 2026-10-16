namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

/// <summary>
/// Единица работы: сохраняет накопленные изменения
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Асинхронно сохраняет все изменения контекста
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}