using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="CityRepository"/>
/// </summary>
public class CityRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICityRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public CityRepositoryTests()
    {
        repository = new CityRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет городов
    /// </summary>
    [Fact]
    public async Task GetCitiesShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetCitiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
    
    
    /// <summary>
    /// Должен вернуть всех неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetCityReturnAllNotDeletedItems()
    {
        // Arrange
        var firstCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        var secondCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstCity, secondCity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetCitiesAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstCity.Id)
            .And.Contain(x => x.Id == secondCity.Id);
    }
    
    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedCity);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCityByIdAsync(deletedCity.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetCitiesShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        var deletedCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeCity, deletedCity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetCitiesAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeCity.Id);
    }
    
    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetCity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        await Context.AddAsync(targetCity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCityByIdAsync(targetCity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCity.Id);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetCityByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}