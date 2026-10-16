using System.Reflection;
using CarDelivery_AcceptanceActApi.Controllers;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests;

public class DependenciesTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public DependenciesTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestAppConfiguration());
    }

    /// <summary>
    /// Проверка резолва зависимостей контроллеров из DI-контейнера
    /// </summary>
    [Theory]
    [MemberData(nameof(Controllers))]
    public void ControllerCoreShouldBeResolved(Type controller)
    {
        using var scope = factory.Services.CreateScope();
        var instance = scope.ServiceProvider.GetRequiredService(controller);
        Assert.NotNull(instance);
    }

    /// <summary>
    /// Контроллеры
    /// </summary>
    public static TheoryData<Type> Controllers => GetControllers<AcceptanceActController>();

    private static TheoryData<Type> GetControllers<TController>() =>
        new(Assembly.GetAssembly(typeof(TController))
                ?.DefinedTypes
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
            ?? []);
}