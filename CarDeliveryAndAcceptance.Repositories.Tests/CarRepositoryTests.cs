using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="CarRepository"/>
/// </summary>
public class CarRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICarRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public CarRepositoryTests()
    {
        repository = new CarRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет машин
    /// </summary>
    [Fact]
    public async Task GetCarsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetCarsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
    
    
    /// <summary>
    /// Должен вернуть всех неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetCarReturnAllNotDeletedItems()
    {
        // Arrange
        var firstCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = null);
        var secondCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstCar, secondCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetCarsAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstCar.Id)
            .And.Contain(x => x.Id == secondCar.Id);
    }
    
    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedCar);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByIdAsync(deletedCar.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetCarsShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = null);
        var deletedCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeCar, deletedCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetCarsAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeCar.Id);
    }
    
    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = null);
        await Context.AddAsync(targetCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByIdAsync(targetCar.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCar.Id);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetCarByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}