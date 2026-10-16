using CarDeliveryAndAcceptance.Context;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Infrastructure;


/// <summary>
/// Базовый класс для интеграционных тестов
/// Предоставляет доступ к БД, UnitOfWork и HTTP-клиенту
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public abstract class IntegrationTestBase
{
    private readonly CarDeliveryAndAcceptanceFixture fixture;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="IntegrationTestBase"/>
    /// </summary>
    protected IntegrationTestBase(CarDeliveryAndAcceptanceFixture fixture)
    {
        this.fixture = fixture;
        Context = fixture.Context;
        UnitOfWork = fixture.UnitOfWork;
    }

    /// <summary>
    /// Контекст БД из DI-контейнера
    /// </summary>
    protected CarDeliveryAndAcceptanceContext Context { get; }

    /// <summary>
    /// UnitOfWork для сохранения изменений
    /// </summary>
    protected IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Создаёт HTTP-клиент для запросов к API
    /// </summary>
    protected HttpClient CreateClient() => fixture.CreateClient();
}