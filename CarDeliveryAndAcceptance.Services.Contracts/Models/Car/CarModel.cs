using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;

namespace CarDeliveryAndAcceptance.Services.Contracts.Models.Car;

public class CarModel : CarCreateModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
}