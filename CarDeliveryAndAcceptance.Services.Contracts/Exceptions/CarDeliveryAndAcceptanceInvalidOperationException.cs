namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, возникающее при недопустимой операции
/// </summary>
public class CarDeliveryAndAcceptanceInvalidOperationException(string message) : CarDeliveryAndAcceptanceException(message);
