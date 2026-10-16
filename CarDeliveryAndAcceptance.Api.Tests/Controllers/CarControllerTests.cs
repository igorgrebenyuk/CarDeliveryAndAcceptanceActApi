using System.Net;
using System.Net.Http.Json;
using CarDeliveryAndAcceptanceActApi.Models;
using CarDeliveryAndAcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using CarDeliveryAndAcceptance.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты CarController
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class CarControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarControllerTests"/>
    /// </summary>
    public CarControllerTests(CarDeliveryAndAcceptanceFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// GET /api/cars возвращает все автомобили
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllCars()
    {
        var expected = await SeedCarAsync();
        await SeedCarAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/api/cars");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cars = await response.Content.ReadFromJsonAsync<List<CarApiModel>>();
        cars.Should().NotBeNull();
        cars!.Should().Contain(x => x.Id == expected.Id && x.VinCode == expected.VinCode);
    }

    /// <summary>
    /// GET /api/cars/{id} возвращает автомобиль
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsCar()
    {
        var seeded = await SeedCarAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/api/cars/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var car = await response.Content.ReadFromJsonAsync<CarApiModel>();
        car.Should().NotBeNull();
        car!.Id.Should().Be(seeded.Id);
        car.Mark.Should().Be(seeded.Mark);
        car.Model.Should().Be(seeded.Model);
        car.LicensePlate.Should().Be(seeded.LicensePlate);
        car.VinCode.Should().Be(seeded.VinCode);
        car.ManufactureYear.Should().Be(seeded.ManufactureYear);
    }

    /// <summary>
    /// GET /api/cars/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/cars/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// GET /api/cars/{id} с не-Guid возвращает 404 (ограничение маршрута)
    /// </summary>
    [Fact]
    public async Task GetById_WhenIdIsNotGuid_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/cars/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /api/cars создаёт автомобиль и сохраняет в БД
    /// </summary>
    [Fact]
    public async Task Create_CreatesCar()
    {
        var request = ValidRequest();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<CarApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.VinCode.Should().Be(request.VinCode);
        created.LicensePlate.Should().Be(request.LicensePlate);

        var inDb = await Context.Set<Car>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.Mark.Should().Be(request.Mark);
        inDb.Model.Should().Be(request.Model);
        inDb.ManufactureYear.Should().Be(request.ManufactureYear);
    }

    /// <summary>
    /// POST /api/cars с существующим VIN-кодом возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenVinExists_Returns406()
    {
        var seeded = await SeedCarAsync();
        var request = ValidRequest();
        request.VinCode = seeded.VinCode;

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        var detail = await response.Content.ReadFromJsonAsync<ApiExceptionDetail>();
        detail!.Message.Should().Contain("VIN");
    }

    /// <summary>
    /// POST /api/cars с существующим регистрационным знаком возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenLicensePlateExists_Returns406()
    {
        var seeded = await SeedCarAsync();
        var request = ValidRequest();
        request.LicensePlate = seeded.LicensePlate;

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        var detail = await response.Content.ReadFromJsonAsync<ApiExceptionDetail>();
        detail!.Message.Should().Contain("регистрационным знаком");
    }

    /// <summary>
    /// POST /api/cars с пустой моделью возвращает 422 и ошибки по обязательным полям
    /// </summary>
    [Fact]
    public async Task Create_WhenInvalid_Returns422WithErrors()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", new CarCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Errors.Should().Contain(x => x.Message.Contains("Марка автомобиля обязательна"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Модель автомобиля обязательна"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Регистрационный знак обязателен"));
        detail.Errors.Should().Contain(x => x.Message.Contains("VIN-код обязателен"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Год выпуска"));
    }

    /// <summary>
    /// POST /api/cars с VIN-кодом длиннее 17 символов возвращает 422
    /// </summary>
    [Fact]
    public async Task Create_WhenVinTooLong_Returns422()
    {
        var request = ValidRequest();
        request.VinCode = new string('A', 18);

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail!.Errors.Should().Contain(x => x.Message.Contains("VIN-код"));
    }

    /// <summary>
    /// POST /api/cars с некорректным годом выпуска возвращает 422
    /// </summary>
    [Fact]
    public async Task Create_WhenYearTooEarly_Returns422()
    {
        var request = ValidRequest();
        request.ManufactureYear = 1800;

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cars", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail!.Errors.Should().Contain(x => x.Message.Contains("Год выпуска указан некорректно"));
    }

    /// <summary>
    /// PUT /api/cars/{id} обновляет автомобиль
    /// </summary>
    [Fact]
    public async Task Update_UpdatesCar()
    {
        var seeded = await SeedCarAsync();
        var request = ValidRequest();
        request.Color = "Красный";

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cars/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Car>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.VinCode.Should().Be(request.VinCode);
        inDb.LicensePlate.Should().Be(request.LicensePlate);
        inDb.Color.Should().Be("Красный");
    }

    /// <summary>
    /// PUT /api/cars/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cars/{Guid.NewGuid()}", ValidRequest());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /api/cars/{id} с VIN-кодом другого автомобиля возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenVinTakenByOther_Returns406()
    {
        var first = await SeedCarAsync();
        var second = await SeedCarAsync();
        var request = ValidRequest();
        request.VinCode = first.VinCode;

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cars/{second.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /api/cars/{id} с регистрационным знаком другого автомобиля возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenLicensePlateTakenByOther_Returns406()
    {
        var first = await SeedCarAsync();
        var second = await SeedCarAsync();
        var request = ValidRequest();
        request.LicensePlate = first.LicensePlate;

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cars/{second.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /api/cars/{id} с невалидной моделью возвращает 422
    /// </summary>
    [Fact]
    public async Task Update_WhenInvalid_Returns422()
    {
        var seeded = await SeedCarAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cars/{seeded.Id}", new CarCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    /// <summary>
    /// DELETE /api/cars/{id} мягко удаляет автомобиль
    /// </summary>
    [Fact]
    public async Task Delete_DeletesCar()
    {
        var seeded = await SeedCarAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/cars/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Car>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/api/cars/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /api/cars/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/cars/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static CarCreateApiModel ValidRequest()
    {
        var suffix = Guid.NewGuid().ToString("N");
        return new CarCreateApiModel
        {
            Mark = "Toyota",
            Model = "Camry",
            LicensePlate = suffix[..10],
            VinCode = suffix[..17],
            ManufactureYear = 2020,
            EngineNumber = suffix[..8],
            ChassisNumber = suffix[..8],
            BodyNumber = suffix[..8],
            Color = "Чёрный",
        };
    }

    private async Task<Car> SeedCarAsync()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var entity = new Car
        {
            Id = Guid.NewGuid(),
            Mark = "Honda",
            Model = "Civic",
            LicensePlate = suffix[..10],
            VinCode = suffix[..17],
            ManufactureYear = 2018,
            EngineNumber = suffix[..8],
            ChassisNumber = suffix[..8],
            BodyNumber = suffix[..8],
            Color = "Белый",
        };
        Context.Set<Car>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}