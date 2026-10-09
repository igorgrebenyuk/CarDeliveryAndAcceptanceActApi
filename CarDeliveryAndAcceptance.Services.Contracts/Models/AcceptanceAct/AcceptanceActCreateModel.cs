namespace CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

public class AcceptanceActCreateModel
{
    
    /// <summary>
    /// Уникальный идентификатор города
    /// </summary>
    public Guid CityId { get; set; }
    
    
    /// <summary>
    /// Уникальный идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор продавца
    /// </summary>
    public Guid SalesmanId { get; set; }

    /// <summary>
    /// Уникальный идентификатор автомобиля
    /// </summary>
    public Guid CarId { get; set; }
}