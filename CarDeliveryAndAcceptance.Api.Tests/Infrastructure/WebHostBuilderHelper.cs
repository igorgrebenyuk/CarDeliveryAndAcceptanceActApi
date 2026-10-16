using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CarDeliveryAndAcceptance.Api.Tests.Infrastructure;


/// <summary>
/// Помощник для конфигурации тестового хоста
/// Настраивает окружение и подключает тестовый конфиг для интеграционных тестов
/// </summary>
static internal class WebHostBuilderHelper
{
    /// <summary>
    ///  Устанавливает окружение "integration",
    ///  подключает appsettings.integration.json и переменные окружения
    /// </summary>

    static internal void ConfigureTestAppConfiguration(this IWebHostBuilder builder)
    {
        builder.UseEnvironment("integration");
        builder.ConfigureAppConfiguration ((_, config) =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.integration.json");
            config.AddJsonFile(configPath).AddEnvironmentVariables();
        });
    }
}