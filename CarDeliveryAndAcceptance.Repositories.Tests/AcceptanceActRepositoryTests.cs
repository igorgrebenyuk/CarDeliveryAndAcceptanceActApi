using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;

namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="AcceptanceActRepository"/>
/// </summary>
public class AcceptanceActRepositoryTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly IAcceptanceActRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов
    /// </summary>
    public AcceptanceActRepositoryTests()
    {
        repository = new AcceptanceActRepository(WriterContext, Context);
    }

    /// <summary>
    /// Создаёт акт вместе со связанными City, Buyer, Salesman и Car и добавляет всё в контекст.
    /// </summary>
    private async Task<AcceptanceAct> CreateActAsync(DateTimeOffset? deletedAt = null)
    {
        var city = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        var buyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        var salesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        var car = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = null);

        var act = TestEntityProvider.Shared.Create<AcceptanceAct>(
            x =>
            {
                x.CityId = city.Id;
                x.City = city;
                x.BuyerId = buyer.Id;
                x.Buyer = buyer;
                x.SalesmanId = salesman.Id;
                x.Salesman = salesman;
                x.CarId = car.Id;
                x.Car = car;
                x.DeletedAt = deletedAt;
            });

        await Context.AddAsync(act);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return act;
    }

    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет актов
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Должен вернуть все неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstAct = await CreateActAsync();
        var secondAct = await CreateActAsync();

        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstAct.Id)
            .And.Contain(x => x.Id == secondAct.Id);
    }

    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedAct = await CreateActAsync(DateTimeOffset.UtcNow);

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(deletedAct.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeAct = await CreateActAsync();
        await CreateActAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeAct.Id);
    }

    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetAct = await CreateActAsync();

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(targetAct.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetAct.Id);
    }

    /// <summary>
    /// Возвращает акт вместе со связанными городом, покупателем, продавцом и автомобилем
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldIncludeRelatedEntities()
    {
        // Arrange
        var targetAct = await CreateActAsync();
        Context.ChangeTracker.Clear();

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(targetAct.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.City.Should().NotBeNull();
        result.Buyer.Should().NotBeNull();
        result.Salesman.Should().NotBeNull();
        result.Car.Should().NotBeNull();
        result.Car.Id.Should().Be(targetAct.CarId);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Должен вернуть акт по сочетанию автомобиля, покупателя и продавца
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByParticipantsShouldReturnEntity()
    {
        // Arrange
        await CreateActAsync();
        var target = await CreateActAsync();

        // Act
        var result = await repository.GetAcceptanceActByParticipantsAsync(
            target.CarId, target.BuyerId, target.SalesmanId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(target.Id);
    }

    /// <summary>
    /// Должен вернуть null, если акт с таким сочетанием удалён или не существует
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByParticipantsShouldReturnNullWhenDeletedOrMissing()
    {
        // Arrange
        var deleted = await CreateActAsync(DateTimeOffset.UtcNow);

        // Act
        var byDeleted = await repository.GetAcceptanceActByParticipantsAsync(
            deleted.CarId, deleted.BuyerId, deleted.SalesmanId, CancellationToken.None);
        var byMissing = await repository.GetAcceptanceActByParticipantsAsync(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        // Assert
        byDeleted.Should().BeNull();
        byMissing.Should().BeNull();
    }
}