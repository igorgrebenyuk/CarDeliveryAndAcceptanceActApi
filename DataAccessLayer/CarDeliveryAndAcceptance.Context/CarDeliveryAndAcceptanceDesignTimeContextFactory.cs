using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarDeliveryAndAcceptance.Context;


/// <summary>
/// Фабрика для создания контекста в DesignTime
/// </summary>
public class CarDeliveryAndAcceptanceDesignTimeContextFactory : IDesignTimeDbContextFactory<CarDeliveryAndAcceptanceContext>
{
    public CarDeliveryAndAcceptanceContext CreateDbContext(string[] args)
    {
        /// <summary>
        /// Creates a new instance of a derived context
        /// </summary>
        /// <remarks>
        /// 1) dotnet tool install --global dotnet-ef
        /// 2) dotnet tool update --global dotnet-ef
        /// 3) dotnet ef migrations add [name] --project DataAccessLayer/CarDeliveryAndAcceptance.Context/CarDeliveryAndAcceptance.Context.csproj
        /// 4) dotnet ef database update --project DataAccessLayer/CarDeliveryAndAcceptance.Context/CarDeliveryAndAcceptance.Context.csproj
        /// 5) dotnet ef database update [targetMigrationName] --project DataAccessLayer/CarDeliveryAndAcceptance.Context/CarDeliveryAndAcceptance.Context.csproj
        /// </remarks>
        var connectionString = ("Host=localhost;Port=5432;Database=CarDeliveryAndAcceptanceActApi;Username=postgres;Password=");
        var options = new DbContextOptionsBuilder<CarDeliveryAndAcceptanceContext>()
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine)
            .Options;
        
        return new CarDeliveryAndAcceptanceContext(options);
    }
}