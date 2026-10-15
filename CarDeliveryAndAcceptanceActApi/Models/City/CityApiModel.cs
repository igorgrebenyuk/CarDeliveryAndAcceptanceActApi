namespace CarDelivery_AcceptanceActApi.Models.City;

/// <summary>
/// Модель ответа
/// </summary>
public class CityApiModel : CityCreateApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}