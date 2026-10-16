namespace CarDeliveryAndAcceptanceActApi.Models.AcceptanceAct;

/// <summary>
/// Модель создания / обновления акта приёма-передачи
/// </summary>
public class AcceptanceActCreateApiModel
{
    /// <summary>
    /// Идентификатор города
    /// </summary>
    public Guid CityId { get; set; }
 
    /// <summary>
    /// Идентификатор покупателя
    /// </summary>
    public Guid BuyerId { get; set; }
 
    /// <summary>
    /// Идентификатор продавца
    /// </summary>
    public Guid SalesmanId { get; set; }
 
    /// <summary>
    /// Идентификатор автомобиля
    /// </summary>
    public Guid CarId { get; set; }
}