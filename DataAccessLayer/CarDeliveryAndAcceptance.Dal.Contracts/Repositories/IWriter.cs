using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

/// <summary>
/// Интерфейс создания и модификации записей в контексте
/// </summary>
public interface IWriter
{
    /// <summary>
    /// Добавить новую запись
    /// </summary>
    void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Изменить запись
    /// </summary>
    void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Удалить запись
    /// </summary>
    void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;
}