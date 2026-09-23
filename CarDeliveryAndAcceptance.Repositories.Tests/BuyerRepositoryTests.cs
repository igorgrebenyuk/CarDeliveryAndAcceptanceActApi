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
    
    
}