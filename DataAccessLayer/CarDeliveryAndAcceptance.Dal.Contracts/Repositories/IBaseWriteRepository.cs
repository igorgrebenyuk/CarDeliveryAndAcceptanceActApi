using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

/// <summary>
/// Базовый интерфейс репозитория записи для сущности <typeparamref name="T"/>
/// </summary>
public interface IBaseWriteRepository<T> where T : class, IEntity
{
    /// <summary>
    ///
    /// </summary>
    void Add(T entity);

    /// <summary>
    ///
    /// </summary>
    void Update(T entity);

    /// <summary>
    ///
    /// </summary>
    void Delete(T entity);
}