using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибках валидации модели
/// </summary>
public class CarDeliveryAndAcceptanceValidationException : CarDeliveryAndAcceptanceException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarDeliveryAndAcceptanceValidationException"/>
    /// </summary>
    public CarDeliveryAndAcceptanceValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}