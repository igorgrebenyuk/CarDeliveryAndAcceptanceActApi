namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

public class EntityNotFoundException<T>(Guid id) : NotFoundException($"Сущность '{typeof(T).Name}' с идентификатором '{id}' не найдена");
