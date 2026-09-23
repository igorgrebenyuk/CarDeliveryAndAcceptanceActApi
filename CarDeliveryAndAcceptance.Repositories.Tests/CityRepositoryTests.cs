using FluentAssertions;
using Xunit;
using Ahatornn.TestGenerator;
using CarDeliveryAndAcceptance.Context.Tests;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Entities;


namespace CarDeliveryAndAcceptance.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="CityRepository"/>
/// </summary>
public class CityRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICityRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public CityRepositoryTests()
    {
        repository = new CityRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет городов
    /// </summary>
    [Fact]
    public async Task GetCitiesShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetCitiesAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
}