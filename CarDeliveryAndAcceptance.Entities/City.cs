using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

public class City : BaseAuditEntity
{
    
    /// <summary>
    /// Название города
    /// </summary>
    public string CityName { get; set; } = string.Empty;
}