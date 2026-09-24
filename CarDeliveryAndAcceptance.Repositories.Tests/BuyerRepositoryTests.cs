using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="BuyerRepository"/>
/// </summary>
public class BuyerRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly IBuyerRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public BuyerRepositoryTests()
    {
        repository = new BuyerRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет покупателей
    /// </summary>
    [Fact]
    public async Task GetBuyersShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetBuyersAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
    
    /// <summary>
    /// Должен вернуть всех неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetBuyerReturnAllNotDeletedItems()
    {
        // Arrange
        var firstBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        var secondBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstBuyer, secondBuyer);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetBuyersAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstBuyer.Id)
            .And.Contain(x => x.Id == secondBuyer.Id);
    }
    
    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedBuyer);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetBuyerByIdAsync(deletedBuyer.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetBuyersShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        var deletedBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeBuyer, deletedBuyer);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetBuyersAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeBuyer.Id);
    }
    
    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetBuyer = TestEntityProvider.Shared.Create<Buyer>(x => x.DeletedAt = null);
        await Context.AddAsync(targetBuyer);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetBuyerByIdAsync(targetBuyer.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetBuyer.Id);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetBuyerByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetBuyerByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}