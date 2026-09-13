using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

/// <summary>
/// Сущность, представляющая акт приема-передачи автомобиля
/// </summary>
public class AcceptanceAct : BaseAuditEntity
{

    /// <summary>
    /// Дата и время создания акта
    /// </summary>
    public DateTimeOffset DateOfCreation { get; set; }

    /// <summary>
    /// Уникальный идентификатор города
    /// </summary>
    public Guid CityId { get; set; }

    /// <summary>
    /// Город составления акта
    /// </summary>
    public virtual City City { get; set; }

    /// <summary>
    /// Уникальный идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Покупатель
    /// </summary>
    public virtual Buyer Buyer { get; set; }

    /// <summary>
    /// Уникальный идентификатор продавца
    /// </summary>
    public Guid SalesmanId { get; set; }

    /// <summary>
    /// Продавец
    /// </summary>
    public virtual Salesman Salesman { get; set; }

    /// <summary>
    /// Уникальный идентификатор автомобиля
    /// </summary>
    public Guid CarId { get; set; }

    /// <summary>
    /// Принимаемый/передаваемый автомобиль
    /// </summary>
    public virtual Car Car { get; set; }
}