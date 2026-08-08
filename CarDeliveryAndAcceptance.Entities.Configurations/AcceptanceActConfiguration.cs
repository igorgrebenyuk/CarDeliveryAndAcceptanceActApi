using FinalExercise.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDeliveryAndAcceptance.Entities.Configurations;

/// <summary>
/// Конфигурация для сущности <see cref="AcceptanceAct" /> для Entity Framework Core
/// </summary>
public class AcceptanceActConfiguration :  IEntityTypeConfiguration<AcceptanceActConfiguration>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AcceptanceActConfiguration> builder)
    {
        builder.ToTable("AcceptanceActs");
    }
}