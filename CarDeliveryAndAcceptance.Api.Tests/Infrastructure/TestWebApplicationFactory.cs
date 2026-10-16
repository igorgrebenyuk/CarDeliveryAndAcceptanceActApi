using CarDeliveryAndAcceptance.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CarDeliveryAndAcceptance.Api.Tests.Infrastructure;


    /// <summary>
    /// Фабрика тестового веб-приложения
    /// Поднимает API в памяти и подменяет DbContext на тестовую БД
    /// </summary>
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        /// <summary>
        /// Настраивает веб-хост: подключает тестовый конфиг и подменяет DbContext
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestAppConfiguration();
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<CarDeliveryAndAcceptanceContext>>();
                services.AddSingleton<DbContextOptions<CarDeliveryAndAcceptanceContext>>(serviceProvider =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    var connectionString = configuration.GetConnectionString("DefaultConnection")!;
                    var dbContextOptions = new DbContextOptions<CarDeliveryAndAcceptanceContext>(
                        new Dictionary<Type, IDbContextOptionsExtension>());
                    var optionsBuilder = new DbContextOptionsBuilder<CarDeliveryAndAcceptanceContext>(dbContextOptions)
                        .UseApplicationServiceProvider(serviceProvider)
                        .UseNpgsql(string.Format(connectionString, Guid.NewGuid().ToString("N")));
                    return optionsBuilder.Options;
                });
            });
        }
    }
