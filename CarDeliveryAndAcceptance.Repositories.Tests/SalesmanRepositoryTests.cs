using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="SalesmanRepository"/>
/// </summary>
public class SalesmanRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ISalesmanRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public SalesmanRepositoryTests()
    {
        repository = new SalesmanRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет продавцов
    /// </summary>
    [Fact]
    public async Task GetSalesmansShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetSalesmanAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
    
    
    /// <summary>
    /// Должен вернуть всех неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetSalesmanReturnAllNotDeletedItems()
    {
        // Arrange
        var firstSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        var secondSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstSalesman, secondSalesman);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetSalesmanAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstSalesman.Id)
            .And.Contain(x => x.Id == secondSalesman.Id);
    }
    
    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedSalesman);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSalesmanByIdAsync(deletedSalesman.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetSalesmansShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        var deletedSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeSalesman, deletedSalesman);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetSalesmanAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeSalesman.Id);
    }
    
    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetSalesman = TestEntityProvider.Shared.Create<Salesman>(x => x.DeletedAt = null);
        await Context.AddAsync(targetSalesman);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSalesmanByIdAsync(targetSalesman.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetSalesman.Id);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetSalesmanByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetSalesmanByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}