using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

namespace CarDeliveryAndAcceptance.Context.Repositories;

/// <summary>
/// Базовый класс репозитория записи данных
/// </summary>
public abstract class BaseWriteRepository<T> where T : class, IEntity
{
    private readonly IDbWriterContext writerContext;
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BaseWriteRepository{T}"/>
    /// </summary>
    protected BaseWriteRepository(IDbWriterContext writerContext)
    {
        this.writerContext = writerContext;
    }
    
    public void Add(T entity)
    {
        if (entity is IEntityWithId entityWithId &&
            entityWithId.Id == Guid.Empty)
        {
            entityWithId.Id = Guid.NewGuid();
        }

        AuditForCreate(entity);
        AuditForUpdate(entity);
        writerContext.Writer.Add(entity);
    }
    
    
    /// <summary>
    /// Обновляет сущность, заполняя аудит изменения
    /// </summary>
    /// <param name="entity">Обновляемая сущность</param>
    public void Update(T entity)
    {
        AuditForUpdate(entity);
        writerContext.Writer.Update(entity);
    }

    /// <summary>
    /// Удаляет сущность: для сущностей с аудитом удаления выполняет мягкое удаление
    /// </summary>
    /// <param name="entity">Удаляемая сущность</param>
    public void Delete(T entity)
    {
        if (entity is IEntityAuditDeletedAt)
        {
            AuditForUpdate(entity);
            AuditForDelete(entity);
            writerContext.Writer.Update(entity);
        }
        else
        {
            writerContext.Writer.Delete(entity);
        }
    }
    
    
    private void AuditForCreate(T entity)
    {
        if (entity is IEntityAuditCreated auditCreated)
        {
            auditCreated.CreatedAt = writerContext.DateTimeProvider.UtcNow;
            auditCreated.CreatedBy = writerContext.IdentityProvider.Name;
        }
    }
    
    private void AuditForUpdate(T entity)
    {
        if (entity is IEntityAuditUpdate auditUpdate)
        {
            auditUpdate.UpdatedAt = writerContext.DateTimeProvider.UtcNow;
            auditUpdate.UpdatedBy = writerContext.IdentityProvider.Name;
        }
    }

    private void AuditForDelete(T entity)
    {
        if (entity is IEntityAuditDeletedAt auditDeleted)
        {
            auditDeleted.DeletedAt = writerContext.DateTimeProvider.UtcNow;
        }
    }
}