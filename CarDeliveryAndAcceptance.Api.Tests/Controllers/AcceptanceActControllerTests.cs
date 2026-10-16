using System.Net;
using System.Net.Http.Json;
using CarDelivery_AcceptanceActApi.Models;
using CarDelivery_AcceptanceActApi.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using CarDeliveryAndAcceptance.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты AcceptanceActController
/// Акт зависит от города, покупателя, продавца и автомобиля — их создаёт <see cref="SeedDependenciesAsync"/>
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class AcceptanceActControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AcceptanceActControllerTests"/>
    /// </summary>
    public AcceptanceActControllerTests(CarDeliveryAndAcceptanceFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// GET /api/acceptance-acts возвращает краткий список актов
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllActs()
    {
        var expected = await SeedActAsync();
        await SeedActAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/api/acceptance-acts");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var acts = await response.Content.ReadFromJsonAsync<List<AcceptanceActListItemApiModel>>();
        acts.Should().NotBeNull();
        acts!.Should().Contain(x => x.Id == expected.Id);
    }

    /// <summary>
    /// GET /api/acceptance-acts/{id} возвращает акт со вложенными сущностями
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsActWithRelatedEntities()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/api/acceptance-acts/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var act = await response.Content.ReadFromJsonAsync<AcceptanceActApiModel>();
        act.Should().NotBeNull();
        act!.Id.Should().Be(seeded.Id);
        act.City.Id.Should().Be(seeded.CityId);
        act.Buyer.Id.Should().Be(seeded.BuyerId);
        act.Salesman.Id.Should().Be(seeded.SalesmanId);
        act.Car.Id.Should().Be(seeded.CarId);
    }

    /// <summary>
    /// GET /api/acceptance-acts/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/acceptance-acts/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /api/acceptance-acts создаёт акт
    /// </summary>
    [Fact]
    public async Task Create_CreatesAct()
    {
        var deps = await SeedDependenciesAsync();
        var request = ToRequest(deps);

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/acceptance-acts", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<AcceptanceActApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.Car.Id.Should().Be(request.CarId);
        created.Buyer.Id.Should().Be(request.BuyerId);

        var inDb = await Context.Set<AcceptanceAct>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.CityId.Should().Be(request.CityId);
        inDb.SalesmanId.Should().Be(request.SalesmanId);
    }

    /// <summary>
    /// POST /api/acceptance-acts с дублем (авто + покупатель + продавец) возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenDuplicate_Returns406()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/acceptance-acts", new AcceptanceActCreateApiModel
        {
            CityId = seeded.CityId,
            BuyerId = seeded.BuyerId,
            SalesmanId = seeded.SalesmanId,
            CarId = seeded.CarId,
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// POST /api/acceptance-acts с несуществующим автомобилем возвращает 404
    /// </summary>
    [Fact]
    public async Task Create_WhenCarNotFound_Returns404()
    {
        var deps = await SeedDependenciesAsync();
        var request = ToRequest(deps);
        request.CarId = Guid.NewGuid();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/acceptance-acts", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /api/acceptance-acts с пустыми идентификаторами возвращает 422 и список ошибок
    /// </summary>
    [Fact]
    public async Task Create_WhenInvalid_Returns422WithErrors()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/acceptance-acts", new AcceptanceActCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Errors.Should().HaveCountGreaterThanOrEqualTo(4);
        detail.Errors.Should().Contain(x => x.Message.Contains("города обязателен"));
        detail.Errors.Should().Contain(x => x.Message.Contains("автомобиля обязателен"));
    }

    /// <summary>
    /// PUT /api/acceptance-acts/{id} обновляет акт
    /// </summary>
    [Fact]
    public async Task Update_UpdatesAct()
    {
        var seeded = await SeedActAsync();
        var newCar = await SeedCarAsync();
        var request = new AcceptanceActCreateApiModel
        {
            CityId = seeded.CityId,
            BuyerId = seeded.BuyerId,
            SalesmanId = seeded.SalesmanId,
            CarId = newCar.Id,
        };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/acceptance-acts/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<AcceptanceAct>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.CarId.Should().Be(newCar.Id);
    }

    /// <summary>
    /// PUT /api/acceptance-acts/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var deps = await SeedDependenciesAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/acceptance-acts/{Guid.NewGuid()}", ToRequest(deps));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /api/acceptance-acts/{id} с комбинацией другого акта возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenCombinationTakenByOther_Returns406()
    {
        var first = await SeedActAsync();
        var second = await SeedActAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/acceptance-acts/{second.Id}", new AcceptanceActCreateApiModel
        {
            CityId = second.CityId,
            BuyerId = first.BuyerId,
            SalesmanId = first.SalesmanId,
            CarId = first.CarId,
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// DELETE /api/acceptance-acts/{id} мягко удаляет акт
    /// </summary>
    [Fact]
    public async Task Delete_DeletesAct()
    {
        var seeded = await SeedActAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/acceptance-acts/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<AcceptanceAct>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/api/acceptance-acts/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /api/acceptance-acts/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/acceptance-acts/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static AcceptanceActCreateApiModel ToRequest(
        (City city, Buyer buyer, Salesman salesman, Car car) deps) => new()
    {
        CityId = deps.city.Id,
        BuyerId = deps.buyer.Id,
        SalesmanId = deps.salesman.Id,
        CarId = deps.car.Id,
    };

    private async Task<Car> SeedCarAsync()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var car = new Car
        {
            Id = Guid.NewGuid(),
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
        Context.Set<Car>().Add(car);
        await UnitOfWork.SaveChangesAsync();
        return car;
    }

    private async Task<(City city, Buyer buyer, Salesman salesman, Car car)> SeedDependenciesAsync()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var city = new City { Id = Guid.NewGuid(), Name = $"Город-{suffix}"[..30] };
        var buyer = new Buyer
        {
            Id = Guid.NewGuid(),
            Name = $"Покупатель-{suffix}",
            FirstName = "Иван",
            Surname = "Иванов",
        };
        var salesman = new Salesman
        {
            Id = Guid.NewGuid(),
            Name = $"Продавец-{suffix}",
            FirstName = "Пётр",
            Surname = "Петров",
        };
        Context.Set<City>().Add(city);
        Context.Set<Buyer>().Add(buyer);
        Context.Set<Salesman>().Add(salesman);
        await UnitOfWork.SaveChangesAsync();

        var car = await SeedCarAsync();
        return (city, buyer, salesman, car);
    }

    private async Task<AcceptanceAct> SeedActAsync()
    {
        var (city, buyer, salesman, car) = await SeedDependenciesAsync();
        var entity = new AcceptanceAct
        {
            Id = Guid.NewGuid(),
            DateOfCreation = DateTimeOffset.UtcNow,
            CityId = city.Id,
            BuyerId = buyer.Id,
            SalesmanId = salesman.Id,
            CarId = car.Id,
        };
        Context.Set<AcceptanceAct>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}