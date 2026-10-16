namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, возникающее, когда запрошенная сущность не найдена
/// </summary>
public class  NotFoundException(string message) : CarDeliveryAndAcceptanceException(message);
