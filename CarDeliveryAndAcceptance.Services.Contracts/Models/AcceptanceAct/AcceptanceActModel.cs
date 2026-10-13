using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

namespace CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

/// <summary>
/// Модель представляющая акт приема-передачи автомобиля
/// </summary>
public class AcceptanceActModel : AcceptanceActCreateModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата и время создания акта
    /// </summary>
    public DateTimeOffset DateOfCreation { get; set; }
    
    /// <summary>
    /// Город составления акта
    /// </summary>
    public CityModel City { get; set; }  = null!;

    /// <summary>
    /// Покупатель
    /// </summary>
    public  BuyerModel Buyer { get; set; }  = null!;
    
    /// <summary>
    /// Продавец
    /// </summary>
    public  SalesmanModel Salesman { get; set; } = null!;

    /// <summary>
    /// Принимаемый/передаваемый автомобиль
    /// </summary>
    public CarModel Car { get; set; } = null!;
}