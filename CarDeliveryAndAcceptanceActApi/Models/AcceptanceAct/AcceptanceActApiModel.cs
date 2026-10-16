using CarDeliveryAndAcceptanceActApi.Models.Buyer;
using CarDeliveryAndAcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptanceActApi.Models.City;
using CarDeliveryAndAcceptanceActApi.Models.Salesman;

namespace CarDeliveryAndAcceptanceActApi.Models.AcceptanceAct;

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