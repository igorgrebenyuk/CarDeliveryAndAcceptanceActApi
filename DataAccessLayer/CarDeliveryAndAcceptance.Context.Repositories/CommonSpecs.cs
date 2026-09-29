using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using CarDeliveryAndAcceptance.Dal.Contracts.Interfaces;

namespace CarDeliveryAndAcceptance.Context.Repositories;

/// <summary>
/// Общие спецификации чтения
/// </summary>
public static class CommonSpecs
{
    /// <summary>
    /// Возвращает <see cref="IReadOnlyCollection{TEntity}"/>
    /// </summary>
    public static Task<IReadOnlyCollection<TEntity>> ToReadOnlyCollectionAsync<TEntity>(this IQueryable<TEntity> query,
        CancellationToken cancellationToken)
        => query.ToListAsync(cancellationToken)
            .ContinueWith(x => new ReadOnlyCollection<TEntity>(x.Result) as IReadOnlyCollection<TEntity>,
                cancellationToken);

    /// <summary>
    /// Активные. Не удаленные.
    /// </summary>
    public static IQueryable<TEntity> NotDeletedAt<TEntity>(this IQueryable<TEntity> query)
        where TEntity : class, IEntityAuditDeletedAt
        => query.Where(x => x.DeletedAt == null);

    /// <summary>
    /// По идентификатору
    /// </summary>
    public static IQueryable<TEntity> ById<TEntity>(this IQueryable<TEntity> query, Guid id)
        where TEntity : class, IEntityWithId
        => query.Where(x => x.Id == id);
}