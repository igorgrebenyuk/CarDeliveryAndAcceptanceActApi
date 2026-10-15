using CarDeliveryAndAcceptance.Common;

namespace CarDelivery_AcceptanceActApi.Implementations;

public class DateTimeProvider: IDateTimeProvider
{
    DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;

    DateTime IDateTimeProvider.LocalNow => DateTime.Now;
}