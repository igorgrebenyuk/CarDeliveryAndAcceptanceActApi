using Ahatornn.TestGenerator;
using AutoMapper;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для <see cref="SalesmanService"/>
/// </summary>
public class SalesmanServiceTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ISalesmanService salesmanService;

    /// <summary>
    /// ctor.
    /// </summary>
    public SalesmanServiceTests()
    {
        var repository = new SalesmanRepository(WriterContext, Context);

        var profile = new ServiceProfile();
        var mapper = new MapperConfiguration(x => x.AddProfile(profile), NullLoggerFactory.Instance).CreateMapper();

        salesmanService = new SalesmanService(repository, UnitOfWork, mapper);
    }

    
    
    /// <summary>
    /// Возвращает пустую коллекцию, если записей нет
    /// </summary>
    [Fact]
    public async Task GetSalesmanShouldReturnEmpty()
    {
        // Act
        var items = await salesmanService.GetSalesmanAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// GetSalesmanAsync возвращает только неудалённые записи
    /// </summary>
    [Fact]
    public async Task GetSalesmanShouldReturnOnlyNotDeletedItems()
    {
        // Arrange
        var first = await AddSalesmanAsync();
        var second = await AddSalesmanAsync();
        await AddSalesmanAsync(DateTimeOffset.UtcNow);

        // Act
        var items = await salesmanService.GetSalesmanAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == first.Id)
            .And.ContainSingle(x => x.Id == second.Id);
    }

    /// <summary>
    /// Возвращает модель со всеми полями
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldReturnValue()
    {
        // Arrange
        var entity = await AddSalesmanAsync();

        // Act
        var result = await salesmanService.GetSalesmanByIdAsync(entity.Id, CancellationToken.None);

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
    /// GetSalesmanByIdAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldThrowNotFoundException()
    {
        // Arrange
        await AddSalesmanAsync();

        // Act
        var act = () => salesmanService.GetSalesmanByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
    }

    /// <summary>
    /// GetSalesmanByIdAsync выбрасывает исключение для удалённой записи
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldThrowNotFoundExceptionWhenDeleted()
    {
        // Arrange
        var deleted = await AddSalesmanAsync(DateTimeOffset.UtcNow);

        // Act
        var act = () => salesmanService.GetSalesmanByIdAsync(deleted.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
    }

    /// <summary>
    /// GetSalesmanByNameAsync возвращает запись по названию
    /// </summary>
    [Fact]
    public async Task GetSalesmanByNameShouldReturnValue()
    {
        // Arrange
        await AddSalesmanAsync();
        var entity = await AddSalesmanAsync();

        // Act
        var result = await salesmanService.GetSalesmanByNameAsync(entity.Name, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
    }

    /// <summary>
    /// GetSalesmanByNameAsync возвращает null, если запись не найдена
    /// (пока тип возврата не nullable)
    /// </summary>
    [Fact]
    public async Task GetSalesmanByNameShouldReturnNullWhenNotFound()
    {
        // Arrange
        await AddSalesmanAsync();

        // Act
        var result = await salesmanService.GetSalesmanByNameAsync("not-existing-name", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// CreateSalesmanAsync сохраняет запись в базе
    /// </summary>
    [Fact]
    public async Task CreateSalesmanShouldWork()
    {
        // Arrange
        var createModel = TestEntityProvider.Shared.Create<SalesmanCreateModel>();

        // Act
        await salesmanService.CreateSalesmanAsync(createModel, CancellationToken.None);

        // Assert
        Context.Set<Salesman>().Should().BeEquivalentTo([new
        {
            createModel.Name,
            createModel.FirstName,
            createModel.Surname
        }]);
    }

    /// <summary>
    /// UpdateSalesmanAsync обновляет поля записи
    /// </summary>
    [Fact]
    public async Task UpdateSalesmanShouldWork()
    {
        // Arrange
        var entity = await AddSalesmanAsync();
        var updateModel = TestEntityProvider.Shared.Create<SalesmanModel>(x => x.Id = entity.Id);

        // Act
        await salesmanService.UpdateSalesmanAsync(updateModel, CancellationToken.None);

        // Assert
        Context.Set<Salesman>().Should().BeEquivalentTo([new
        {
            updateModel.Id,
            updateModel.Name,
            updateModel.FirstName,
            updateModel.Surname
        }]);
    }

    /// <summary>
    /// UpdateSalesmanAsync выбрасывает исключение, если запись не найдена
    /// </summary>
    [Fact]
    public async Task UpdateSalesmanShouldThrowNotFoundException()
    {
        // Arrange
        await AddSalesmanAsync();
        var updateModel = TestEntityProvider.Shared.Create<SalesmanModel>(x => x.Id = Guid.NewGuid());

        // Act
        var act = () => salesmanService.UpdateSalesmanAsync(updateModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
    }

    /// <summary>
    /// DeleteSalesmanAsync помечает запись удалённой
    /// </summary>
    [Fact]
    public async Task DeleteSalesmanShouldWork()
    {
        // Arrange
        var entity = await AddSalesmanAsync();

        // Act
        await salesmanService.DeleteSalesmanAsync(entity.Id, CancellationToken.None);

        // Assert
        var deleted = Context.Set<Salesman>().Single(x => x.Id == entity.Id);
        deleted.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// DeleteSalesmanAsync выбрасывает исключение, если запись не найдена, и не трогает существующие записи
    /// </summary>
    [Fact]
    public async Task DeleteSalesmanShouldThrowNotFoundException()
    {
        // Arrange
        var entity = await AddSalesmanAsync();

        // Act
        var act = () => salesmanService.DeleteSalesmanAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Salesman>>();
        Context.Set<Salesman>().Single(x => x.Id == entity.Id).DeletedAt.Should().BeNull();
    }
    
    private async Task<Salesman> AddSalesmanAsync(DateTimeOffset? deletedAt = null)
    {
        var entity = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = deletedAt);
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        return entity;
    }
}