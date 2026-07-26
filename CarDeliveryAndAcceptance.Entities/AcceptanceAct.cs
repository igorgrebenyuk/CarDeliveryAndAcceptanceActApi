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
    public string IdCity { get; set; }

    /// <summary>
    /// Дата проведения Акта приемки-передачи
    /// </summary>
    public DateTimeOffset DateOfCreation { get; set; }

    /// <summary>
    /// Уникальный идентификатор Покупателя
    /// </summary>
    public string IdBuyer { get; set; }

    /// <summary>
    /// Уникальный идентификатор Продавца
    /// </summary>
    public string IdSalesman { get; set; }

    /// <summary>
    /// Уникальный идентификатор Продавца
    /// </summary>
    public string IdCar { get; set; }
}