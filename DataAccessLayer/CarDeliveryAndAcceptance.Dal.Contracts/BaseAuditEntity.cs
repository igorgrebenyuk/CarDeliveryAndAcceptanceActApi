using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Dal.Contracts;

public abstract class BaseAuditEntity : 
    IEntity,
    IEntityWithId,
    IEntityAuditCreated,
    IEntityAuditUpdate,
    IEntityAuditDeletedAt
{
    /// <summary>
    /// Уникальный идентификатор автомобиля
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Когда создан
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем создан
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Когда изменён
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кем изменён
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}