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
    /// Инициализирует новый экземпляр тестов репозитория дисциплин
    /// </summary>
    public AcceptanceActRepositoryTests()
    {
        repository = new AcceptanceActRepository(WriterContext, Context);
    }
    
    
}