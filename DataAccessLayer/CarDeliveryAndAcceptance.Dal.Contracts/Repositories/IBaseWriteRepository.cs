using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

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