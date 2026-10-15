namespace CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;

/// <summary>
/// Представляет модель покупателя
/// </summary>
public class BuyerModel : BuyerCreateModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
}