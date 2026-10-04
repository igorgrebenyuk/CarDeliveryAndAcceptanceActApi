namespace CarDeliveryAndAcceptance.Services.Contracts.Models;

public class CityModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название города
    /// </summary>
    public string Name { get; set; } = string.Empty;
}