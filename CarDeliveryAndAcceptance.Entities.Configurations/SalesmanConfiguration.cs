using CarDeliveryAndAcceptance.Context.EntityFrameworkCore;
using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDeliveryAndAcceptance.Entities.Configurations;

/// <summary>
/// Конфигурация для сущности <see cref="Salesman" /> для Entity Framework Core
/// </summary>
public class SalesmanConfiguration:  IEntityTypeConfiguration<Salesman>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Salesman> builder)
    {
        builder.ToTable("Salesmans");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();
        
        builder.Property(x => x.SalesmanName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.SalesmanFirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SalesmanSurname)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(x => new { x.SalesmanName, x.SalesmanFirstName, x.SalesmanSurname })
            .HasDatabaseName("IX_Salesmen_FullName")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
}
