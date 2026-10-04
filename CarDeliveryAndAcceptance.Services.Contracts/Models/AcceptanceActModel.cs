namespace CarDeliveryAndAcceptance.Services.Contracts.Models;

/// <summary>
/// Модель представляющая акт приема-передачи автомобиля
/// </summary>
public class AcceptanceActModel
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
    /// Уникальный идентификатор города
    /// </summary>
    public Guid CityId { get; set; }

    /// <summary>
    /// Город составления акта
    /// </summary>
    public virtual CityModel CityModel { get; set; }

    /// <summary>
    /// Уникальный идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Покупатель
    /// </summary>
    public virtual BuyerModel BuyerModel { get; set; }

    /// <summary>
    /// Уникальный идентификатор продавца
    /// </summary>
    public Guid SalesmanId { get; set; }

    /// <summary>
    /// Продавец
    /// </summary>
    public virtual SalesmanModel SalesmanModel { get; set; }

    /// <summary>
    /// Уникальный идентификатор автомобиля
    /// </summary>
    public Guid CarId { get; set; }

    /// <summary>
    /// Принимаемый/передаваемый автомобиль
    /// </summary>
    public virtual CarModel CarModel { get; set; }
}