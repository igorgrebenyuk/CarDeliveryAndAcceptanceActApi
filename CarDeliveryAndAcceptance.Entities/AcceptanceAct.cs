using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

public class AcceptanceAct : BaseAuditEntity
{
    /// <summary>
    /// Уникальный идентификатор Акта приемки-передачи
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор Города
    /// </summary>
    public Guid IdCity { get; set; }

    /// <summary>
    /// Дата проведения Акта приемки-передачи
    /// </summary>
    public DateTimeOffset DateOfCreation { get; set; }

    /// <summary>
    /// Уникальный идентификатор Покупателя
    /// </summary>
    public Guid IdBuyer { get; set; }

    /// <summary>
    /// Уникальный идентификатор Продавца
    /// </summary>
    public Guid IdSalesman { get; set; }

    /// <summary>
    /// Уникальный идентификатор Продавца
    /// </summary>
    public Guid IdCar { get; set; }
}