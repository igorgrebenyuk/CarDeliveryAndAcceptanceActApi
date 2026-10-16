namespace CarDeliveryAndAcceptanceActApi.Models.Car;

/// <summary>
/// Модель ответа
/// </summary>
public class CarApiModel : CarCreateApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}