using FinalExercise.Context.EntityFrameworkCore;
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
        
    }
}
