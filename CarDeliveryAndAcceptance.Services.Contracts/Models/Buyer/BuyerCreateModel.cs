namespace CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;

public class BuyerCreateModel
{
    /// <summary>
    /// Название покупателя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Имя покупателя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
        
    /// <summary>
    /// Фамилия покупателя
    /// </summary>
    public string Surname { get; set; } = string.Empty;
}