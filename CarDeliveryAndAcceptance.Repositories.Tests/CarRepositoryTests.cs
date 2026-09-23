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
public class CarRepositoryTests: CarDeliveryAndAcceptanceContextInMemory
{
    private readonly ICarRepository repository;
    
    
    
    /// <summary>
    /// Инициализирует новый экземпляр тестов репозитория актов 
    /// </summary>
    public CarRepositoryTests()
    {
        repository = new CarRepository(WriterContext, Context);
    }
    
    /// <summary>
    /// Должен вернуть пустую коллекцию если в базе нет машин
    /// </summary>
    [Fact]
    public async Task GetCarsShouldReturnEmpty()
    {
        // Act
        var items = await repository.GetCarsAsync(CancellationToken.None);

        // Assert
        items.Should().NotBeNull().And.BeEmpty();
        
    }
}