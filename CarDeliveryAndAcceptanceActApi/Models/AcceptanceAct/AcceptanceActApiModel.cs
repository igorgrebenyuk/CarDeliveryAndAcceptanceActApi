using CarDelivery_AcceptanceActApi.Models.Buyer;
using CarDelivery_AcceptanceActApi.Models.Car;
using CarDelivery_AcceptanceActApi.Models.City;
using CarDelivery_AcceptanceActApi.Models.Salesman;

namespace CarDelivery_AcceptanceActApi.Models.AcceptanceAct;

/// <summary>
/// Модель акта приёма-передачи (ответ API)
/// </summary>
public class AcceptanceActApiModel 
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
    /// Город
    /// </summary>
    public CityApiModel City { get; set; } = null!;
 
    /// <summary>
    /// Покупатель
    /// </summary>
    public BuyerApiModel Buyer { get; set; } = null!;
 
    /// <summary>
    /// Продавец
    /// </summary>
    public SalesmanApiModel Salesman { get; set; } = null!;
 
    /// <summary>
    /// Автомобиль
    /// </summary>
    public CarApiModel Car { get; set; } = null!;
}