using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

public class CarDeliveryAndAcceptanceValidationException : CarDeliveryAndAcceptanceException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    public CarDeliveryAndAcceptanceValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}