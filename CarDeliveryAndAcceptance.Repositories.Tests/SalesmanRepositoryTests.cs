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
}