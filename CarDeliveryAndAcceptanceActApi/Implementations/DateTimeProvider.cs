using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptanceActApi.Implementations;

/// <summary>
/// Реализация <see cref="IDateTimeProvider"/>
/// </summary>
public class DateTimeProvider: IDateTimeProvider
{
    DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;

    DateTime IDateTimeProvider.LocalNow => DateTime.Now;
}