using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptanceActApi.Models;

public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
}