
namespace CarDeliveryAndAcceptance.Services.Contracts.Models.Car;

/// <summary>
/// Модель автомобиля
/// </summary>
public class CarModel : CarCreateModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
}