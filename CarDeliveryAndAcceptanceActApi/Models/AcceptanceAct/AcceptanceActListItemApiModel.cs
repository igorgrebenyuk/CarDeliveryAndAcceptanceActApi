namespace CarDeliveryAndAcceptanceActApi.Models.AcceptanceAct;

/// <summary>
/// Краткая модель акта приёма-передачи для списка
/// </summary>
public class AcceptanceActListItemApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
 
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset DateOfCreation { get; set; }
 
    /// <summary>
    /// Название города
    /// </summary>
    public string CityName { get; set; } = string.Empty;
 
    /// <summary>
    /// Покупатель
    /// </summary>
    public string BuyerFullName { get; set; } = string.Empty;
 
    /// <summary>
    /// Продавец
    /// </summary>
    public string SalesmanFullName { get; set; } = string.Empty;
 
    /// <summary>
    /// Марка автомобиля
    /// </summary>
    public string CarMark { get; set; } = string.Empty;
 
    /// <summary>
    /// Модель автомобиля
    /// </summary>
    public string CarModel { get; set; } = string.Empty;
 
    /// <summary>
    /// Государственный номер автомобиля
    /// </summary>
    public string CarLicensePlate { get; set; } = string.Empty;
}