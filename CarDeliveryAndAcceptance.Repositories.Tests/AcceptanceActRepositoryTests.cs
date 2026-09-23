using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="AcceptanceActRepository"/>
/// </summary>
public class AcceptanceActRepositoryTests : CarDeliveryAndAcceptanceContextInMemory
{
    private readonly IAcceptanceActRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public AcceptanceActRepositoryTests()
    {
        repository = new AcceptanceActRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет актов
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
    
    /// <summary>
    /// Должен вернуть все неудаленные акты, если они есть в базе
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldReturnAllNotDeletedItems()
    {
        // Arrange
        var firstAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = null);
        var secondAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = null);
        await Context.AddRangeAsync(firstAct, secondAct);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(2)
            .And.Contain(x => x.Id == firstAct.Id)
            .And.Contain(x => x.Id == secondAct.Id);
    }
    
    /// <summary>
    /// Возвращает null, если акт помечен как удаленный
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnNullWhenEntityIsDeleted()
    {
        // Arrange
        var deletedAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(deletedAct);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(deletedAct.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    /// <summary>
    /// Не должен возвращать акты, помеченные как удаленные
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActsShouldNotReturnDeletedItems()
    {
        // Arrange
        var activeAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = null);
        var deletedAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(activeAct, deletedAct);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);
 
        // Act
        var items = await repository.GetAcceptanceActsAsync(CancellationToken.None);
 
        // Assert
        items.Should().NotBeNull()
            .And.HaveCount(1)
            .And.OnlyContain(x => x.Id == activeAct.Id);
    }
    
    /// <summary>
    /// Возвращает акт по идентификатору, если он существует и не удален
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnEntityWhenExistsAndNotDeleted()
    {
        // Arrange
        var targetAct = TestEntityProvider.Shared.Create<AcceptanceAct>(x => x.DeletedAt = null);
        await Context.AddAsync(targetAct);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(targetAct.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(targetAct.Id);
    }

    /// <summary>
    /// Возвращает null, если акт с указанным идентификатором не найден
    /// </summary>
    [Fact]
    public async Task GetAcceptanceActByIdShouldReturnNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetAcceptanceActByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
   
   
}