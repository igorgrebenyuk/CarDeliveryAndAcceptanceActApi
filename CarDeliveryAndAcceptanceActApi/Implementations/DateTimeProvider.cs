using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptanceActApi.Implementations;

public class DateTimeProvider: IDateTimeProvider
{
    DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;

    DateTime IDateTimeProvider.LocalNow => DateTime.Now;
}