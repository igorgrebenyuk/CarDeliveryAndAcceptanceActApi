using CarDeliveryAndAcceptance.Common;

namespace CarDelivery_AcceptanceActApi.Models;

public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
}