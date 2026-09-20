using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarDeliveryAndAcceptance.Context.Tests;

/// <summary>
/// Контекст <see cref="CarDeliveryAndAcceptance.Context"/> для тестов с базой в памяти. Один контекст на тест.
/// </summary>
public class CarDeliveryAndAcceptanceContextInMemory : IAsyncDisposable
{
    /// <summary>
    /// Контекст <see cref="CarDeliveryAndAcceptanceContext"/>
    /// </summary>
    protected CarDeliveryAndAcceptanceContext Context { get; }

    /// <inheritdoc cref="IUnitOfWork"/>
    protected IUnitOfWork UnitOfWork => Context;

    /// <inheritdoc cref="IDbWriterContext"/>
    protected IDbWriterContext WriterContext => new TestWriterContext(Context);

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="FinalExerciseContextInMemory"/>
    /// </summary>
    protected CarDeliveryAndAcceptanceContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarDeliveryAndAcceptanceContext>()
            .UseInMemoryDatabase($"CarDeliveryAndAcceptanceContextTests{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        Context = new CarDeliveryAndAcceptanceContext(optionsBuilder.Options);
    }

    /// <inheritdoc cref="IAsyncDisposable"/>
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
}