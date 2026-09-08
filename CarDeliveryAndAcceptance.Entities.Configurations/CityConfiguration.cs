using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarDeliveryAndAcceptance.Entities;
using FinalExercise.Context.EntityFrameworkCore;
using FinalExercise.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Entities.Configurations;

/// <summary>
/// Конфигурация для сущности <see cref="City" /> для Entity Framework Core
/// </summary>
public class CityConfiguration  : IEntityTypeConfiguration<City>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<City> builder)
    {
        // Имя таблицы в единственном или множественном числе
        builder.ToTable("Cities");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        
        // Настройка названия города
        builder.Property(x => x.CityName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(x => x.CityName)
            .HasDatabaseName("IX_Cities_CityName")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }

}