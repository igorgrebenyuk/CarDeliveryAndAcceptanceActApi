namespace CarDeliveryAndAcceptanceActApi.Models.Salesman;

/// <summary>
/// Модель ответа
/// </summary>
public class SalesmanApiModel : SalesmanCreateApiModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}