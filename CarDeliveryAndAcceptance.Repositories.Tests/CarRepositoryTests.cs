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
public class CarRepositoryTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICarRepository repository;
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория автомобилей 
    /// </summary>
    public CarRepositoryTests()
    {
        repository = new CarRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию, если в базе нет машин
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
    /// Должен вернуть все неудаленные автомобили, если они есть в базе
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
    /// Возвращает null, если автомобиль помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetCarByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedCar = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None); // Исправлено на UnitOfWork

        // Act
        var result = await repository.GetCarByIdAsync(deletedCar.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать автомобили, помеченные как удаленные
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
    /// Возвращает автомобиль по идентификатору, если он существует и не удален
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
    /// Возвращает null, если автомобиль с указанным идентификатором не найден
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

    /// <summary>
    /// Возвращает автомобиль по VIN-коду, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetCarByVinCodeShouldReturnThisCarWhenExistsAndNotDeleted()
    {
        // Arrange
        // 1. Целевой VinCode
        var targetCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = null;
            x.VinCode = "JHMCM56557C404453";
        });

        // 2. МАШИНА-ШУМ: Тот же VIN-код, но удаленная 
        var deletedCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = DateTimeOffset.UtcNow; 
            x.VinCode = "JHMCM56557C404453";
        });

        // 3. МАШИНА-ШУМ: Другой VIN-код 
        var otherCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = null;
            x.VinCode = "1HGCR2F83HA000000";
        });
        
        await Context.AddRangeAsync(targetCar, deletedCar, otherCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        
        // Act
        var result = await repository.GetCarByVinCodeAsync(targetCar.VinCode, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(targetCar.Id); 
        result.DeletedAt.Should().BeNull();
    }
    
    /// <summary>
    /// Возвращает null, если автомобиль с указанным VIN-кодом не найден
    /// </summary>
    [Fact]
    public async Task GetCarByVinCodeShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var otherCar = TestEntityProvider.Shared.Create<Car>(x => {
            x.DeletedAt = null;
            x.VinCode = "1HGCR2F83HA000000";
        });
        await Context.AddAsync(otherCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByVinCodeAsync("JHMCM56557C404453", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Должен возвращать автомобиль независимо от регистра символов в VIN-коде
    /// </summary>
    [Fact]
    public async Task GetCarByVinCodeShouldBeCaseInsensitive()
    {
        // Arrange
        var targetCar = TestEntityProvider.Shared.Create<Car>(x => {
            x.DeletedAt = null;
            x.VinCode = "1HGCR2F83HA000000";
        });
        await Context.AddAsync(targetCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByVinCodeAsync("1hgcr2f83ha000000", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCar.Id);
    }
    
    /// <summary>
    /// Возвращает автомобиль по госномеру, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetCarByLicensePlateShouldReturnThisCarWhenExistsAndNotDeleted()
    {
        // Arrange
        // 1. Целевой госномер
        var targetCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = null;
            x.LicensePlate = "Е777КХ99";
        });

        // 2. МАШИНА-ШУМ: Тот же госномер, но удаленная 
        var deletedCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = DateTimeOffset.UtcNow; 
            x.LicensePlate = "Е777КХ99";
        });

        // 3. МАШИНА-ШУМ: Другой госномер 
        var otherCar = TestEntityProvider.Shared.Create<Car>(settings: x => {
            x.DeletedAt = null;
            x.LicensePlate = "А123АА77";
        });
        
        await Context.AddRangeAsync(targetCar, deletedCar, otherCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        
        // Act
        var result = await repository.GetCarByLicensePlateAsync(targetCar.LicensePlate, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(targetCar.Id); 
        result.DeletedAt.Should().BeNull();
    }
    
    /// <summary>
    /// Возвращает null, если автомобиль с указанным госномер не найден
    /// </summary>
    [Fact]
    public async Task GetCarByLicensePlateShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var otherCar = TestEntityProvider.Shared.Create<Car>(x => {
            x.DeletedAt = null;
            x.LicensePlate = "А123АА77";
        });
        await Context.AddAsync(otherCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByLicensePlateAsync("Е777КХ99", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Должен возвращать автомобиль независимо от регистра символов в госномере
    /// </summary>
    [Fact]
    public async Task GetCarByLicensePlateShouldBeCaseInsensitive()
    {
        // Arrange
        var targetCar = TestEntityProvider.Shared.Create<Car>(x => {
            x.DeletedAt = null;
            x.LicensePlate = "Е777КХ99";
        });
        await Context.AddAsync(targetCar);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetCarByLicensePlateAsync("е777кх99", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetCar.Id);
    }
}
