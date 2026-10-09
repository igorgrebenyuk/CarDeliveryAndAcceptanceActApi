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
    public virtual CityModel CityModel { get; set; }

    /// <summary>
    /// Покупатель
    /// </summary>
    public virtual BuyerModel BuyerModel { get; set; }
    
    /// <summary>
    /// Продавец
    /// </summary>
    public virtual SalesmanModel SalesmanModel { get; set; }

    /// <summary>
    /// Принимаемый/передаваемый автомобиль
    /// </summary>
    public virtual CarModel CarModel { get; set; }
}