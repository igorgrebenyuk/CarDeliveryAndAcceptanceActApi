using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptanceActApi.Models;

/// <summary>
/// Детали ошибки валидации для ответа API
/// </summary>
public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
}