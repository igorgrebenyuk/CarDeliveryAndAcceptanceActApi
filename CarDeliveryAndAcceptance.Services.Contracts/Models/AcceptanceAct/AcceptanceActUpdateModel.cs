namespace CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

/// <summary>
/// Модель для обновления акта приема-передачи
/// </summary>
public class AcceptanceActUpdateModel : AcceptanceActCreateModel
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid Id { get; set; }
}