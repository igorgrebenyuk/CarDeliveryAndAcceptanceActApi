namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

public interface IUnitOfWork
{
    /// <summary>
    /// Асинхронно сохраняет все изменения контекста
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}