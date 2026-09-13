using CarDeliveryAndAcceptance.Context.EntityFrameworkCore;
using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;
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

        builder.Property(x => x.Mark)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(100);
        
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
        
        
        builder.HasIndex(x => x.VinCode)
            .HasDatabaseName("IX_Cars_VinCode")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");

        builder.HasIndex(x => x.LicensePlate)
            .HasDatabaseName("IX_Cars_LicensePlate")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
    
}