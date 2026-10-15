using Ahatornn.TestGenerator;
using AutoMapper;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для <see cref="CarService"/>
/// </summary>
public class CarServiceTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICarService carService;

    /// <summary>
    /// ctor.
    /// </summary>
    public CarServiceTests()
    {
        var repository = new CarRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        carService = new CarService(repository, UnitOfWork, mapper);
    }

   

    /// <summary>
    /// GetCarsAsync возвращает пустую коллекцию, если автомобилей нет
    /// </summary>
    [Fact]
    public async Task GetCarsShouldReturnEmpty()
    {
        // Act
        var items = await carService.GetCarsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// GetCarsAsync возвращает только неудалённые автомобили
    /// </summary>
    [Fact]
    public async Task GetCarsShouldReturnOnlyNotDeletedItems()
    {
        // Arrange
        var car1 = await AddCarAsync();
        var car2 = await AddCarAsync();
        await AddCarAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await carService.GetCarsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == car1.Id)
            .And.ContainSingle(x => x.Id == car2.Id);
    }

    /// <summary>
    /// GetCarByIdAsync возвращает модель со всеми полями автомобиля
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldReturnValue()
    {
        // Arrange
        var entity = await AddCarAsync();

        // Act
        var result = await carService.GetCarByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new
        {
            entity.Id,
            entity.Mark,
            entity.Model,
            entity.LicensePlate,
            entity.VinCode,
            entity.ManufactureYear,
            entity.EngineNumber,
            entity.ChassisNumber,
            entity.BodyNumber,
            entity.Color
        });
    }

    /// <summary>
    /// GetCarByIdAsync выбрасывает исключение, если автомобиль не найден
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldThrowNotFoundException()
    {
        // Arrange
        await AddCarAsync();

        // Act
        var act = () => carService.GetCarByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
    }

    /// <summary>
    /// GetCarByIdAsync выбрасывает исключение для удалённого автомобиля
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldThrowNotFoundExceptionWhenDeleted()
    {
        // Arrange
        var deleted = await AddCarAsync(DateTimeOffset.UtcNow);

        // Act
        var act = () => carService.GetCarByIdAsync(deleted.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
    }

    /// <summary>
    /// GetCarByVinCodeAsync возвращает автомобиль по VIN-коду
    /// </summary>
    [Fact]
    public async Task GetCarByVinCodeShouldReturnValue()
    {
        // Arrange
        await AddCarAsync();
        var entity = await AddCarAsync();

        // Act
        var result = await carService.GetCarByVinCodeAsync(entity.VinCode, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.VinCode.Should().Be(entity.VinCode);
    }

    /// <summary>
    /// GetCarByVinCodeAsync возвращает null, если автомобиль не найден 
    /// </summary>
    [Fact]
    public async Task GetCarByVinCodeShouldReturnNullWhenNotFound()
    {
        // Arrange
        await AddCarAsync();

        // Act
        var result = await carService.GetCarByVinCodeAsync("not-existing-vin", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// GetCarByLicensePlateAsync возвращает автомобиль по регистрационному знаку
    /// </summary>
    [Fact]
    public async Task GetCarByLicensePlateShouldReturnValue()
    {
        // Arrange
        await AddCarAsync();
        var entity = await AddCarAsync();

        // Act
        var result = await carService.GetCarByLicensePlateAsync(entity.LicensePlate, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.LicensePlate.Should().Be(entity.LicensePlate);
    }

    /// <summary>
    /// GetCarByLicensePlateAsync возвращает null, если автомобиль не найден
    /// </summary>
    [Fact]
    public async Task GetCarByLicensePlateShouldReturnNullWhenNotFound()
    {
        // Arrange
        await AddCarAsync();

        // Act
        var result = await carService.GetCarByLicensePlateAsync("not-existing-plate", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// CreateCarAsync сохраняет автомобиль в базе
    /// </summary>
    [Fact]
    public async Task CreateCarShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<CarCreateModel>();

        // Act
        await carService.CreateCarAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Car>().Should().BeEquivalentTo([new
        {
            createModel.Mark,
            createModel.Model,
            createModel.LicensePlate,
            createModel.VinCode,
            createModel.ManufactureYear,
            createModel.EngineNumber,
            createModel.ChassisNumber,
            createModel.BodyNumber,
            createModel.Color
        }]);
    }

    /// <summary>
    /// UpdateCarAsync обновляет поля автомобиля
    /// </summary>
    [Fact]
    public async Task UpdateCarShouldWork()
    {
        // Arrange
        var entity = await AddCarAsync();
        var updateModel = TestEntityProvider.Shared.Create<CarModel>(x => x.Id = entity.Id);

        // Act
        await carService.UpdateCarAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Car>().Should().BeEquivalentTo([new
        {
            updateModel.Id,
            updateModel.Mark,
            updateModel.Model,
            updateModel.LicensePlate,
            updateModel.VinCode,
            updateModel.ManufactureYear,
            updateModel.EngineNumber,
            updateModel.ChassisNumber,
            updateModel.BodyNumber,
            updateModel.Color
        }]);
    }

    /// <summary>
    /// UpdateCarAsync выбрасывает исключение, если автомобиль не найден
    /// </summary>
    [Fact]
    public async Task UpdateCarShouldThrowNotFoundException()
    {
        // Arrange
        await AddCarAsync();
        var updateModel = TestEntityProvider.Shared.Create<CarModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => carService.UpdateCarAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
    }

    /// <summary>
    /// DeleteCarAsync помечает автомобиль удалённым
    /// </summary>
    [Fact]
    public async Task DeleteCarShouldWork()
    {
        // Arrange
        var entity = await AddCarAsync();

        // Act
        await carService.DeleteCarAsync(entity.Id, CancellationToken.None);

        // Assert
        var deleted = Context.Set<Car>().Single(x => x.Id == entity.Id);
        deleted.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DeleteCarAsync выбрасывает исключение, если автомобиль не найден, и не трогает существующие записи
    /// </summary>
    [Fact]
    public async Task DeleteCarShouldThrowNotFoundException()
    {
        // Arrange
        var entity = await AddCarAsync();

        // Act
        var act = () => carService.DeleteCarAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
        Context.Set<Car>().Single(x => x.Id == entity.Id).DeletedAt.Should().BeNull();
    }
    
    
    private async Task<Car> AddCarAsync(DateTimeOffset? deletedAt = null)
    {
        var entity = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = deletedAt);
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return entity;
    }
}