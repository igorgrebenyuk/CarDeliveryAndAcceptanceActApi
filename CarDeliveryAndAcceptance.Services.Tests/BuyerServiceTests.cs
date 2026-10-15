using Ahatornn.TestGenerator;
using AutoMapper;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для <see cref="BuyerService"/>
/// </summary>
public class BuyerServiceTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly IBuyerService buyerService;

    /// <summary>
    /// ctor.
    /// </summary>
    public BuyerServiceTests()
    {
        var repository = new BuyerRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        buyerService = new BuyerService(repository, UnitOfWork, mapper);
    }

    
    

    /// <summary>
    /// GetBuyersAsync возвращает пустую коллекцию, если записей нет
    /// </summary>
    [Fact]
    public async Task GetBuyersShouldReturnEmpty()
    {
        // Act
        var items = await buyerService.GetBuyersAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// GetBuyersAsync возвращает только неудалённые записи
    /// </summary>
    [Fact]
    public async Task GetBuyersShouldReturnOnlyNotDeletedItems()
    {
        // Arrange
        var first = await AddBuyerAsync();
        var second = await AddBuyerAsync();
        await AddBuyerAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await buyerService.GetBuyersAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == first.Id)
            .And.ContainSingle(x => x.Id == second.Id);
    }

    /// <summary>
    /// GetBuyerByIdAsync возвращает модель со всеми полями
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldReturnValue()
    {
        // Arrange
        var entity = await AddBuyerAsync();

        // Act
        var result = await buyerService.GetBuyerByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new
        {
            entity.Id,
            entity.Name,
            entity.FirstName,
            entity.Surname
        });
    }

    /// <summary>
    /// GetBuyerByIdAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldThrowNotFoundException()
    {
        // Arrange
        await AddBuyerAsync();

        // Act
        var act = () => buyerService.GetBuyerByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
    }

    /// <summary>
    /// GetBuyerByIdAsync выбрасывает исключение для удалённой записи
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldThrowNotFoundExceptionWhenDeleted()
    {
        // Arrange
        var deleted = await AddBuyerAsync(DateTimeOffset.UtcNow);

        // Act
        var act = () => buyerService.GetBuyerByIdAsync(deleted.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
    }

    /// <summary>
    /// GetBuyerByNameAsync возвращает запись по названию
    /// </summary>
    [Fact]
    public async Task GetBuyerByNameShouldReturnValue()
    {
        // Arrange
        await AddBuyerAsync();
        var entity = await AddBuyerAsync();

        // Act
        var result = await buyerService.GetBuyerByNameAsync(entity.Name, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
    }

    /// <summary>
    /// GetBuyerByNameAsync возвращает null, если запись не найдена
    /// (пока тип возврата не nullable)
    /// </summary>
    [Fact]
    public async Task GetBuyerByNameShouldReturnNullWhenNotFound()
    {
        // Arrange
        await AddBuyerAsync();

        // Act
        var result = await buyerService.GetBuyerByNameAsync("not-existing-name", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// CreateBuyerAsync сохраняет запись в базе
    /// </summary>
    [Fact]
    public async Task CreateBuyerShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<BuyerCreateModel>();

        // Act
        await buyerService.CreateBuyerAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Buyer>().Should().BeEquivalentTo([new
        {
            createModel.Name,
            createModel.FirstName,
            createModel.Surname
        }]);
    }

    /// <summary>
    /// UpdateBuyerAsync обновляет поля записи
    /// </summary>
    [Fact]
    public async Task UpdateBuyerShouldWork()
    {
        // Arrange
        var entity = await AddBuyerAsync();
        var updateModel = TestEntityProvider.Shared.Create<BuyerModel>(x => x.Id = entity.Id);

        // Act
        await buyerService.UpdateBuyerAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Buyer>().Should().BeEquivalentTo([new
        {
            updateModel.Id,
            updateModel.Name,
            updateModel.FirstName,
            updateModel.Surname
        }]);
    }

    /// <summary>
    /// UpdateBuyerAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task UpdateBuyerShouldThrowNotFoundException()
    {
        // Arrange
        await AddBuyerAsync();
        var updateModel = TestEntityProvider.Shared.Create<BuyerModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => buyerService.UpdateBuyerAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
    }

    /// <summary>
    /// DeleteBuyerAsync помечает запись удалённой
    /// </summary>
    [Fact]
    public async Task DeleteBuyerShouldWork()
    {
        // Arrange
        var entity = await AddBuyerAsync();

        // Act
        await buyerService.DeleteBuyerAsync(entity.Id, CancellationToken.None);

        // Assert
        var deleted = Context.Set<Buyer>().Single(x => x.Id == entity.Id);
        deleted.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DeleteBuyerAsync выбрасывает исключение, если запись не найдена, и не трогает существующие записи
    /// </summary>
    [Fact]
    public async Task DeleteBuyerShouldThrowNotFoundException()
    {
        // Arrange
        var entity = await AddBuyerAsync();

        // Act
        var act = () => buyerService.DeleteBuyerAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Buyer>>();
        Context.Set<Buyer>().Single(x => x.Id == entity.Id).DeletedAt.Should().BeNull();
    }
    
    /// <summary>
    /// CreateBuyerAsync возвращает созданную модель
    /// </summary>
    [Fact]
    public async Task CreateBuyerShouldReturnCreatedModel()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<BuyerCreateModel>();

        // Act
        var result = await buyerService.CreateBuyerAsync(createModel, CancellationToken.None);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(createModel.Name);
    }

    /// <summary>
    /// CreateBuyerAsync выбрасывает исключение, если покупатель с таким названием уже существует
    /// </summary>
    [Fact]
    public async Task CreateBuyerShouldThrowWhenNameAlreadyExists()
    {
        // Arrange
        var existing = await AddBuyerAsync();
        var createModel = TestEntityProvider.Shared.Create<BuyerCreateModel>(x => x.Name = existing.Name);

        // Act
        var act = () => buyerService.CreateBuyerAsync(createModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CarDeliveryAndAcceptanceInvalidOperationException>();
        Context.Set<Buyer>().Should().ContainSingle();
    }

    /// <summary>
    /// UpdateBuyerAsync выбрасывает исключение, если название занято другой записью
    /// </summary>
    [Fact]
    public async Task UpdateBuyerShouldThrowWhenNameAlreadyExists()
    {
        // Arrange
        var first = await AddBuyerAsync();
        var second = await AddBuyerAsync();
        var updateModel = TestEntityProvider.Shared.Create<BuyerModel>(x =>
        {
            x.Id = second.Id;
            x.Name = first.Name;
        });

        // Act
        var act = () => buyerService.UpdateBuyerAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CarDeliveryAndAcceptanceInvalidOperationException>();
    }

    /// <summary>
    /// UpdateBuyerAsync позволяет сохранить собственное название записи
    /// </summary>
    [Fact]
    public async Task UpdateBuyerShouldWorkWhenNameIsNotChanged()
    {
        // Arrange
        var entity = await AddBuyerAsync();
        var updateModel = TestEntityProvider.Shared.Create<BuyerModel>(x =>
        {
            x.Id = entity.Id;
            x.Name = entity.Name;
        });

        // Act
        var act = () => buyerService.UpdateBuyerAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    private async Task<Buyer> AddBuyerAsync(DateTimeOffset? deletedAt = null)
    {
        var entity = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = deletedAt);
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return entity;
    }
}