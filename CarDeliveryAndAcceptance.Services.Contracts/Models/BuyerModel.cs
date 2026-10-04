namespace CarDeliveryAndAcceptance.Services.Contracts.Models;

/// <summary>
/// Представляет модель покупателя
/// </summary>
public class BuyerModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
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