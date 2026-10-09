namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

public class EntityNotFoundException<T>(Guid id) : CarDeliveryAndAcceptanceException($"Сущность '{typeof(T).Name}' с идентификатором '{id}' не найдена");
