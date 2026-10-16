using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CarDeliveryAndAcceptanceActApi.Infrastructure;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать несколько реализаций одного интерфейса
    /// </summary>
    /// <param name="services">DI-контейнер</param>
    /// <param name="implementationsAssembly">Сборка, где искать реализации</param>
    /// <param name="lifetime">Время жизни реализаций</param>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    public static void RegisterImplementationsOf<TService>(this IServiceCollection services,
        Assembly implementationsAssembly,
        ServiceLifetime lifetime)
    {
        var serviceType = typeof(TService);

        var types = implementationsAssembly.GetTypes()
            .Where(t => t != serviceType &&
                        serviceType.IsAssignableFrom(t) &&
                        t is
                        {
                            IsAbstract: false,
                            IsPublic: true,
                            IsInterface: false
                        });

        services.TryAddEnumerable(types.Select(x => new ServiceDescriptor(serviceType, x, lifetime)));
    }
}