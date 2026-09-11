using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Context;
 
public class CarDeliveryAndAcceptanceContext : DbContext,
    IReader,
    IWriter,
    IUnitOfWork
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarDeliveryAndAcceptanceContext"/>
    /// </summary>
    public CarDeliveryAndAcceptanceContext(DbContextOptions<CarDeliveryAndAcceptanceContext> options)
        : base(options)
    {
        // https://aspnetzero.com
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", isEnabled: true);
    }
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntitiesAnchor).Assembly);
    }
    IQueryable<TEntity> IReader.Read<TEntity>()
        => base.Set<TEntity>()
            .AsNoTracking();

    void IWriter.Add<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Added;

    void IWriter.Update<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Modified;

    void IWriter.Delete<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Deleted;

    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        var count = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in base.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return count;
    }
}