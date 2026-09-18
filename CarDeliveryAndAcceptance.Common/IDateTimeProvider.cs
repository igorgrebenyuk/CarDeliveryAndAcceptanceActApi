namespace CarDeliveryAndAcceptance.Common;

/// <summary>
/// Интерфейс получения даты
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Текущий момент (utc)
    /// </summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>
    /// Текущий момент (локальное время)
    /// </summary>
    DateTime LocalNow { get; }
}