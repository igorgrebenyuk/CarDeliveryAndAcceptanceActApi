using FinalExercise.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDeliveryAndAcceptance.Entities.Configurations;


/// <summary>
/// Конфигурация для сущности <see cref="Car" /> для Entity Framework Core
/// </summary>
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    /// < inheritdoc />
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        // Настройка свойств (ограничения длины и обязательность)
        builder.Property(x => x.MakeAndModel)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.VinCode)
            .IsRequired()
            .HasMaxLength(17); 

        builder.Property(x => x.ManufactureYear)
            .IsRequired();

        builder.Property(x => x.EngineNumber)
            .HasMaxLength(50);

        builder.Property(x => x.ChassisNumber)
            .HasMaxLength(50);

        builder.Property(x => x.BodyNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Color)
            .HasMaxLength(50);

        // Индексы для оптимизации поиска и обеспечения уникальности
        builder.HasIndex(x => x.LicensePlate)
            .IsUnique(); 

        builder.HasIndex(x => x.VinCode)
            .IsUnique(); // VIN-код всегда уникален
    }
    
}