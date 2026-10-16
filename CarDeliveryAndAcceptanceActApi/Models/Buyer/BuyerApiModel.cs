namespace CarDeliveryAndAcceptanceActApi.Models.Buyer;


/// <summary>
/// Модель покупателя
/// </summary>
public class BuyerApiModel : BuyerCreateApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}