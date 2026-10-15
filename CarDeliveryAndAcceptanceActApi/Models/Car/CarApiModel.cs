namespace CarDelivery_AcceptanceActApi.Models.Car;

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