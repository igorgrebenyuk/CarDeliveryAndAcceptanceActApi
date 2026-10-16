namespace CarDeliveryAndAcceptanceActApi.Models;

/// <summary>
/// Детали ошибки для ответа API
/// </summary>
public class ApiExceptionDetail
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; } = string.Empty;
}