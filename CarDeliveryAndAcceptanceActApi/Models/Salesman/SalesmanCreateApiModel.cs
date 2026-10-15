namespace CarDelivery_AcceptanceActApi.Models.Salesman;

/// <summary>
/// Модель создания
/// </summary>
public class SalesmanCreateApiModel
{
    /// <summary>
    /// Наименование организации
    /// </summary>
    public string Name { get; set; } = string.Empty;
 
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
 
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; } = string.Empty;
}