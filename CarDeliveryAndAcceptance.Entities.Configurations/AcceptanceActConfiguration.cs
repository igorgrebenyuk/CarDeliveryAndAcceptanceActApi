using CarDeliveryAndAcceptance.Context.EntityFrameworkCore;
using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDeliveryAndAcceptance.Entities.Configurations;

/// <summary>
/// Конфигурация для сущности <see cref="AcceptanceAct" /> для Entity Framework Core
/// </summary>
public class AcceptanceActConfiguration :  IEntityTypeConfiguration<AcceptanceAct>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AcceptanceAct> builder)
    {
        builder.ToTable("AcceptanceActs");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();
        
        
        
        builder.HasOne(x => x.City)
            .WithMany() 
            .HasForeignKey(x => x.CityId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
     
        builder.HasOne(x => x.Buyer)
            .WithMany()
            .HasForeignKey(x => x.BuyerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Salesman)
            .WithMany()
            .HasForeignKey(x => x.SalesmanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Car)
            .WithMany()
            .HasForeignKey(x => x.CarId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.CarId, x.BuyerId, x.SalesmanId, x.DateOfCreation })
            .HasDatabaseName("IX_AcceptanceActs")
            .IsUnique()
            .HasFilter($"\"{nameof(IEntityAuditDeletedAt.DeletedAt)}\" IS NULL");
    }
}