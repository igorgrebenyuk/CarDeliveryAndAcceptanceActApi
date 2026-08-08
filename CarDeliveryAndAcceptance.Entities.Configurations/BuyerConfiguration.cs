using FinalExercise.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDeliveryAndAcceptance.Entities.Configurations;

/// <summary>
/// Конфигурация для сущности <see cref="Buyer" /> для Entity Framework Core
/// </summary>
public class BuyerConfiguration :  IEntityTypeConfiguration<Buyer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("Buyers");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        // Настройка свойств (ограничения длины и обязательность)
        builder.Property(x => x.BuyerName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.BuyerFirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.BuyerSurname)
            .IsRequired()
            .HasMaxLength(50);

        
    }
}
