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
    public string CityName { get; set; } = string.Empty;
}