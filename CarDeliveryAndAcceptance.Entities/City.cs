using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

public class City : BaseAuditEntity
{
    /// <summary>
    /// Уникальный идентификатор города
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название города
    /// </summary>
    public string Name { get; set; } = string.Empty;
}