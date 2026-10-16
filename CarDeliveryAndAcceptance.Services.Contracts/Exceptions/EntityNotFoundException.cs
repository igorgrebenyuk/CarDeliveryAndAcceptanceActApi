namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, возникающее, когда сущность типа <typeparamref name="T"/> с указанным идентификатором не найдена
/// </summary>
public class EntityNotFoundException<T>(Guid id) : NotFoundException($"Сущность '{typeof(T).Name}' с идентификатором '{id}' не найдена");
