using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using DocumentFormat.OpenXml.Packaging;
using FluentAssertions;
using Xunit;

namespace CarDeliveryAndAcceptance.Services.Tests;

/// <summary>
/// Тесты для сервиса генерации Excel-файлов актов приёма-передачи
/// </summary>
public class AcceptanceActExcelServiceTests
{
    /// <summary>
    /// GenerateAcceptanceActExcelAsync выполненяет генерацию Excel-файла акта
    /// </summary>
    [Fact]
    public async Task GenerateAcceptanceActExcelAsync_ShouldGenerateValidExcel()
    {
        // Arrange
        var act = CreateAcceptanceActModel();
        var service = new AcceptanceActExcelService();

        // Act
        var stream = await service.GenerateAcceptanceActExcelAsync(
            act,
            CancellationToken.None);

        // Assert
        stream.Should().NotBeNull();
        stream.Length.Should().BeGreaterThan(0);

        using (stream)
        using (var document = SpreadsheetDocument.Open(stream, false))
        {
            document.WorkbookPart.Should().NotBeNull();
            document.WorkbookPart!.Workbook.Should().NotBeNull();
        }
    }

    /// <summary>
    /// GenerateAcceptanceActExcelAsync при пустом акте выбрасывается исключение
    /// </summary>
    [Fact]
    public async Task GenerateAcceptanceActExcelAsync_ShouldThrow_WhenActIsNull()
    {
        // Arrange
        var service = new AcceptanceActExcelService();

        // Act
        var action = () => service.GenerateAcceptanceActExcelAsync(
            null!,
            CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    private AcceptanceActModel CreateAcceptanceActModel()
    {
        return new AcceptanceActModel
        {
            Id = Guid.NewGuid(),
            DateOfCreation = DateTimeOffset.UtcNow,
            City = new CityModel
            {
                Id = Guid.NewGuid(),
                Name = "Москва"
            },
            Salesman = new SalesmanModel
            {
                Id = Guid.NewGuid(),
                Name = "ООО Продавец",
                FirstName = "Андрей",
                Surname = "Надеждин"
            },
            Buyer = new BuyerModel
            {
                Id = Guid.NewGuid(),
                Name = "ООО Покупатель",
                FirstName = "Иван",
                Surname = "Иванов"
            },
            Car = new CarModel
            {
                Id = Guid.NewGuid(),
                Mark = "Toyota",
                Model = "Camry",
                LicensePlate = "А123ВС777",
                VinCode = "JTNB11HK103123456",
                ManufactureYear = 2020,
                EngineNumber = "ENG-001",
                ChassisNumber = "CH-001",
                BodyNumber = "BODY-001",
                Color = "Чёрный"
            }
        };
    }
}