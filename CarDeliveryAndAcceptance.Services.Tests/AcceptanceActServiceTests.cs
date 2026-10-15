using Ahatornn.TestGenerator;
using AutoMapper;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для <see cref="AcceptanceActService"/>
/// </summary>
public class AcceptanceActServiceTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly IAcceptanceActService acceptanceActService;
    
    private sealed record Related(City City, Buyer Buyer, Salesman Salesman, Car Car);

    /// <summary>
    /// ctor.
    /// </summary>
    public AcceptanceActServiceTests()
    {
        var actRepository = new AcceptanceActRepository(WriterContext, Context);
        var carRepository = new CarRepository(WriterContext, Context);
        var buyerRepository = new BuyerRepository(WriterContext, Context);
        var salesmanRepository = new SalesmanRepository(WriterContext, Context);
        var cityRepository = new CityRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        acceptanceActService = new AcceptanceActService(
            actRepository,
            carRepository,
            buyerRepository,
            salesmanRepository,
            cityRepository,
            UnitOfWork,
            mapper);
    }



    /// <summary>
    /// GetAcceptanceActsAsync возвращает пустую коллекцию, если актов нет
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnEmpty()
    {
        // Act
        var items = await acceptanceActService.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// GetAcceptanceActsAsync возвращает только неудалённые акты
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnOnlyNotDeletedItems()
    {
        // Arrange
        var first = await AddActAsync();
        var second = await AddActAsync();
        await AddActAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await acceptanceActService.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == first.Id)
            .And.ContainSingle(x => x.Id == second.Id);
    }

    /// <summary>
    /// GetAcceptanceActByIdAsync возвращает акт вместе со связанными сущностями
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnValueWithRelatedModels()
    {
        // Arrange
        var entity = await AddActAsync();
        Context.ChangeTracker.Clear();

        // Act
        var result = await acceptanceActService.GetAcceptanceActByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.DateOfCreation.Should().Be(entity.CreatedAt);
        result.CityId.Should().Be(entity.CityId);
        result.BuyerId.Should().Be(entity.BuyerId);
        result.SalesmanId.Should().Be(entity.SalesmanId);
        result.CarId.Should().Be(entity.CarId);

        result.City.Should().NotBeNull();
        result.City.Id.Should().Be(entity.CityId);
        result.Buyer.Should().NotBeNull();
        result.Buyer.Id.Should().Be(entity.BuyerId);
        result.Buyer.Name.Should().NotBeNullOrEmpty();
        result.Salesman.Should().NotBeNull();
        result.Salesman.Id.Should().Be(entity.SalesmanId);
        result.Car.Should().NotBeNull();
        result.Car.Id.Should().Be(entity.CarId);
    }

    /// <summary>
    /// GetAcceptanceActByIdAsync выбрасывает исключение, если акт не найден
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldThrowNotFoundException()
    {
        // Arrange
        await AddActAsync();

        // Act
        var act = () => acceptanceActService.GetAcceptanceActByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<AcceptanceAct>>();
    }

    /// <summary>
    /// GetAcceptanceActByIdAsync выбрасывает исключение для удалённого акта
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldThrowNotFoundExceptionWhenDeleted()
    {
        // Arrange
        var deleted = await AddActAsync(DateTimeOffset.UtcNow);

        // Act
        var act = () => acceptanceActService.GetAcceptanceActByIdAsync(deleted.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<AcceptanceAct>>();
    }


    /// <summary>
    /// CreateAcceptanceActAsync сохраняет акт в базе
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldWork()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = BuildCreateModel(related);

        // Act
        await acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<AcceptanceAct>().Should().BeEquivalentTo([new
        {
            createModel.CityId,
            createModel.BuyerId,
            createModel.SalesmanId,
            createModel.CarId
        }]);
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если автомобиль не найден
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenCarNotFound()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = BuildCreateModel(related, carId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
        Context.Set<AcceptanceAct>().Should().BeEmpty();
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если автомобиль удалён
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenCarDeleted()
    {
        // Arrange
        var related = await AddRelatedAsync(carDeleted: true);
        var createModel = BuildCreateModel(related);

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
        Context.Set<AcceptanceAct>().Should().BeEmpty();
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если покупатель не найден
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenBuyerNotFound()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = BuildCreateModel(related, buyerId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
        Context.Set<AcceptanceAct>().Should().BeEmpty();
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если продавец не найден
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenSalesmanNotFound()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = BuildCreateModel(related, salesmanId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
        Context.Set<AcceptanceAct>().Should().BeEmpty();
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если город не найден
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenCityNotFound()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = BuildCreateModel(related, cityId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
        Context.Set<AcceptanceAct>().Should().BeEmpty();
    }



    /// <summary>
    /// UpdateAcceptanceActAsync меняет связанные сущности акта, Id остаётся прежним
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldWork()
    {
        // Arrange
        var entity = await AddActAsync();
        var newRelated = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(entity.Id, newRelated);

        // Act
        await acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<AcceptanceAct>().Should().ContainSingle(x => x.Id == entity.Id);
        var updated = Context.Set<AcceptanceAct>().Single(x => x.Id == entity.Id);
        updated.CityId.Should().Be(newRelated.City.Id);
        updated.BuyerId.Should().Be(newRelated.Buyer.Id);
        updated.SalesmanId.Should().Be(newRelated.Salesman.Id);
        updated.CarId.Should().Be(newRelated.Car.Id);
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если акт не найден
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenActNotFound()
    {
        // Arrange
        await AddActAsync();
        var related = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(Guid.NewGuid(), related);

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<AcceptanceAct>>();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если автомобиль не найден
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenCarNotFound()
    {
        // Arrange
        var entity = await AddActAsync();
        var related = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(entity.Id, related, carId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Car>>();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если покупатель не найден
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenBuyerNotFound()
    {
        // Arrange
        var entity = await AddActAsync();
        var related = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(entity.Id, related, buyerId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если продавец не найден
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenSalesmanNotFound()
    {
        // Arrange
        var entity = await AddActAsync();
        var related = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(entity.Id, related, salesmanId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если город не найден
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenCityNotFound()
    {
        // Arrange
        var entity = await AddActAsync();
        var related = await AddRelatedAsync();
        var updateModel = BuildUpdateModel(entity.Id, related, cityId: Guid.NewGuid());

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<City>>();
    }
    

    /// <summary>
    /// DeleteAcceptanceActAsync помечает акт удалённым
    /// </summary>
    [Fact]
    public async Task DeleteAcceptanceActShouldWork()
    {
        // Arrange
        var entity = await AddActAsync();

        // Act
        await acceptanceActService.DeleteAcceptanceActAsync(entity.Id, CancellationToken.None);

        // Assert
        var deleted = Context.Set<AcceptanceAct>().Single(x => x.Id == entity.Id);
        deleted.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DeleteAcceptanceActAsync выбрасывает исключение, если акт не найден,и не трогает существующие акты
    /// </summary>
    [Fact]
    public async Task DeleteAcceptanceActShouldThrowNotFoundException()
    {
        // Arrange
        var entity = await AddActAsync();

        // Act
        var act = () => acceptanceActService.DeleteAcceptanceActAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<AcceptanceAct>>();
        Context.Set<AcceptanceAct>().Single(x => x.Id == entity.Id).DeletedAt.Should().BeNull();
    }
    
    
        
    /// <summary>
    /// CreateAcceptanceActAsync возвращает созданный акт со вложенными моделями
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldReturnCreatedModelWithRelatedModels()
    {
        // Arrange
        var related = await AddRelatedAsync();
        var createModel = TestEntityProvider.Shared.Create<AcceptanceActCreateModel>(x =>
        {
            x.CarId = related.Car.Id;
            x.BuyerId = related.Buyer.Id;
            x.SalesmanId = related.Salesman.Id;
            x.CityId = related.City.Id;
        });

        // Act
        var result = await acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.Car.Id.Should().Be(related.Car.Id);
        result.Buyer.Id.Should().Be(related.Buyer.Id);
        result.Salesman.Id.Should().Be(related.Salesman.Id);
        result.City.Id.Should().Be(related.City.Id);
    }

    /// <summary>
    /// CreateAcceptanceActAsync выбрасывает исключение, если акт с таким автомобилем, покупателем и продавцом уже существует
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldThrowWhenActWithSameParticipantsExists()
    {
        // Arrange
        var existing = await AddActAsync();
        var createModel = new AcceptanceActCreateModel
        {
            CityId = existing.CityId,
            BuyerId = existing.BuyerId,
            SalesmanId = existing.SalesmanId,
            CarId = existing.CarId
        };

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CarDeliveryAndAcceptanceInvalidOperationException>();
        Context.Set<AcceptanceAct>().Should().ContainSingle();
    }

    /// <summary>
    /// CreateAcceptanceActAsync не считает дублем удалённый акт
    /// </summary>
    [Fact]
    public async Task CreateAcceptanceActShouldWorkWhenSameActIsDeleted()
    {
        // Arrange
        var deleted = await AddActAsync(DateTimeOffset.UtcNow);
        var createModel = new AcceptanceActCreateModel
        {
            CityId = deleted.CityId,
            BuyerId = deleted.BuyerId,
            SalesmanId = deleted.SalesmanId,
            CarId = deleted.CarId
        };

        // Act
        var act = () => acceptanceActService.CreateAcceptanceActAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync выбрасывает исключение, если такое сочетание уже занято другим актом
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldThrowWhenActWithSameParticipantsExists()
    {
        // Arrange
        var first = await AddActAsync();
        var second = await AddActAsync();
        var updateModel = new AcceptanceActUpdateModel
        {
            Id = second.Id,
            CityId = second.CityId,
            BuyerId = first.BuyerId,
            SalesmanId = first.SalesmanId,
            CarId = first.CarId
        };

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CarDeliveryAndAcceptanceInvalidOperationException>();
    }

    /// <summary>
    /// UpdateAcceptanceActAsync позволяет сохранить акт с его же сочетанием автомобиля, покупателя и продавца
    /// </summary>
    [Fact]
    public async Task UpdateAcceptanceActShouldWorkWhenParticipantsAreNotChanged()
    {
        // Arrange
        var entity = await AddActAsync();
        var updateModel = new AcceptanceActUpdateModel
        {
            Id = entity.Id,
            CityId = entity.CityId,
            BuyerId = entity.BuyerId,
            SalesmanId = entity.SalesmanId,
            CarId = entity.CarId
        };

        // Act
        var act = () => acceptanceActService.UpdateAcceptanceActAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    private async Task<Related> AddRelatedAsync(bool carDeleted = false)
    {
        var city = TestEntityProvider.Shared.Create<City>(x => x.DeletedAt = null);
        var buyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        var salesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        var car = TestEntityProvider.Shared.Create<Car>(x => x.DeletedAt = carDeleted ? DateTimeOffset.UtcNow : null);

        Context.AddRange(city, buyer, salesman, car);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        return new Related(city, buyer, salesman, car);
    }

    
    private async Task<AcceptanceAct> AddActAsync(DateTimeOffset? deletedAt = null)
    {
        var related = await AddRelatedAsync();

        var act = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = deletedAt);

        act.CityId = related.City.Id;
        act.City = null!;
        act.BuyerId = related.Buyer.Id;
        act.Buyer = null!;
        act.SalesmanId = related.Salesman.Id;
        act.Salesman = null!;
        act.CarId = related.Car.Id;
        act.Car = null!;

        Context.Add(act);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return act;
    }

    
    private static AcceptanceActCreateModel BuildCreateModel(
        Related related,
        Guid? cityId = null,
        Guid? buyerId = null,
        Guid? salesmanId = null,
        Guid? carId = null)
        => new()
        {
            CityId = cityId ?? related.City.Id,
            BuyerId = buyerId ?? related.Buyer.Id,
            SalesmanId = salesmanId ?? related.Salesman.Id,
            CarId = carId ?? related.Car.Id
        };

    
    private static AcceptanceActUpdateModel BuildUpdateModel(
        Guid id,
        Related related,
        Guid? cityId = null,
        Guid? buyerId = null,
        Guid? salesmanId = null,
        Guid? carId = null)
        => new()
        {
            Id = id,
            CityId = cityId ?? related.City.Id,
            BuyerId = buyerId ?? related.Buyer.Id,
            SalesmanId = salesmanId ?? related.Salesman.Id,
            CarId = carId ?? related.Car.Id
        };

    
}