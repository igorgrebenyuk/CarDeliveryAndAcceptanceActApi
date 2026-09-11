using CarDeliveryAndAcceptance.Context.EntityFrameworkCore;
using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;
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

        builder.Property(x => x.BuyerName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.BuyerFirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.BuyerSurname)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => new { x.BuyerName, x.BuyerFirstName, x.BuyerSurname })
            .HasDatabaseName("IX_Buyers_FullName")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
}
