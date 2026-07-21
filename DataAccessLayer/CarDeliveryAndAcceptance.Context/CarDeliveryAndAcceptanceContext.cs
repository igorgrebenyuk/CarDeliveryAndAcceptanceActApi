using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Context;

public class CarDeliveryAndAcceptanceContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarDeliveryAndAcceptanceContext"/>
    /// </summary>
    public CarDeliveryAndAcceptanceContext(DbContextOptions<CarDeliveryAndAcceptanceContext> options)
        : base(options)
    {
        // https://aspnetzero.com
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", isEnabled: true);
    }

}