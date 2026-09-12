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
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(x => new { x.Name, x.FirstName, x.Surname })
            .HasDatabaseName("IX_Salesmen_FullName")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
}
