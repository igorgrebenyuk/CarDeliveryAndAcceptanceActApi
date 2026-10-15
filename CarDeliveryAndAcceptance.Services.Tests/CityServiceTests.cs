using Ahatornn.TestGenerator;
using AutoMapper;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для <see cref="CityService"/>
/// </summary>
public class CityServiceTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICityService cityService;

    /// <summary>
    /// ctor.
    /// </summary>
    public CityServiceTests()
    {
        var repository = new CityRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        cityService = new CityService(repository, UnitOfWork, mapper);
    }

    

    /// <summary>
    /// Возвращает пустую коллекцию, если записей нет
    /// </summary>
    [Fact]
    public async Task GetCitiesShouldReturnEmpty()
    {
        // Act
        var items = await cityService.GetCitiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Возвращает только неудалённые записи
    /// </summary>
    [Fact]
    public async Task GetCitiesShouldReturnOnlyNotDeletedItems()
    {
        // Arrange
        var first = await AddCityAsync();
        var second = await AddCityAsync();
        await AddCityAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await cityService.GetCitiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == first.Id)
            .And.ContainSingle(x => x.Id == second.Id);
    }

    /// <summary>
    /// GetCityByIdAsync возвращает модель со всеми полями
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldReturnValue()
    {
        // Arrange
        var entity = await AddCityAsync();

        // Act
        var result = await cityService.GetCityByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new
        {
            entity.Id,
            entity.Name
        });
    }

    /// <summary>
    /// GetCityByIdAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldThrowNotFoundException()
    {
        // Arrange
        await AddCityAsync();

        // Act
        var act = () => cityService.GetCityByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
    }

    /// <summary>
    /// GetCityByIdAsync выбрасывает исключение для удалённой записи
    /// </summary>
    [Fact]
    public async Task GetCityByIdShouldThrowNotFoundExceptionWhenDeleted()
    {
        // Arrange
        var deleted = await AddCityAsync(DateTimeOffset.UtcNow);

        // Act
        var act = () => cityService.GetCityByIdAsync(deleted.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
    }

    /// <summary>
    /// GetCityByNameAsync возвращает запись по названию
    /// </summary>
    [Fact]
    public async Task GetCityByNameShouldReturnValue()
    {
        // Arrange
        await AddCityAsync();
        var entity = await AddCityAsync();

        // Act
        var result = await cityService.GetCityByNameAsync(entity.Name, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
    }

    /// <summary>
    /// GetCityByNameAsync возвращает null, если запись не найдена
    /// (пока тип возврата не nullable)
    /// </summary>
    [Fact]
    public async Task GetCityByNameShouldReturnNullWhenNotFound()
    {
        // Arrange
        await AddCityAsync();

        // Act
        var result = await cityService.GetCityByNameAsync("not-existing-name", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// CreateCityAsync сохраняет запись в базе
    /// </summary>
    [Fact]
    public async Task CreateCityShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<CityCreateModel>();

        // Act
        await cityService.CreateCityAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<City>().Should().BeEquivalentTo([new
        {
            createModel.Name
        }]);
    }

    /// <summary>
    /// UpdateCityAsync обновляет поля записи
    /// </summary>
    [Fact]
    public async Task UpdateCityShouldWork()
    {
        // Arrange
        var entity = await AddCityAsync();
        var updateModel = TestEntityProvider.Shared.Create<CityModel>(x => x.Id = entity.Id);

        // Act
        await cityService.UpdateCityAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<City>().Should().BeEquivalentTo([new
        {
            updateModel.Id,
            updateModel.Name
        }]);
    }

    /// <summary>
    /// UpdateCityAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task UpdateCityShouldThrowNotFoundException()
    {
        // Arrange
        await AddCityAsync();
        var updateModel = TestEntityProvider.Shared.Create<CityModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => cityService.UpdateCityAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
    }

    /// <summary>
    /// DeleteCityAsync помечает запись удалённой
    /// </summary>
    [Fact]
    public async Task DeleteCityShouldWork()
    {
        // Arrange
        var entity = await AddCityAsync();

        // Act
        await cityService.DeleteCityAsync(entity.Id, CancellationToken.None);

        // Assert
        var deleted = Context.Set<City>().Single(x => x.Id == entity.Id);
        deleted.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DeleteCityAsync выбрасывает исключение, если запись не найдена, и не трогает существующие записи
    /// </summary>
    [Fact]
    public async Task DeleteCityShouldThrowNotFoundException()
    {
        // Arrange
        var entity = await AddCityAsync();

        // Act
        var act = () => cityService.DeleteCityAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
        Context.Set<City>().Single(x => x.Id == entity.Id).DeletedAt.Should().BeNull();
    }
    
    private async Task<City> AddCityAsync(DateTimeOffset? deletedAt = null)
    {
        var entity = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = deletedAt);
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return entity;
    }
}