using System.Net;
using System.Net.Http.Json;
using CarDeliveryAndAcceptanceActApi.Models;
using CarDeliveryAndAcceptanceActApi.Models.City;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using CarDeliveryAndAcceptance.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты CityController
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class CityControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CityControllerTests"/>
    /// </summary>
    public CityControllerTests(CarDeliveryAndAcceptanceFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// GET /api/cities возвращает все города
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAllCities()
    {
        var expected = await SeedCityAsync();
        await SeedCityAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/api/cities");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cities = await response.Content.ReadFromJsonAsync<List<CityApiModel>>();
        cities.Should().NotBeNull();
        cities!.Should().Contain(x => x.Id == expected.Id && x.Name == expected.Name);
    }

    /// <summary>
    /// GET /api/cities/{id} возвращает город
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsCity()
    {
        var seeded = await SeedCityAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/api/cities/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var city = await response.Content.ReadFromJsonAsync<CityApiModel>();
        city.Should().NotBeNull();
        city!.Id.Should().Be(seeded.Id);
        city.Name.Should().Be(seeded.Name);
    }

    /// <summary>
    /// GET /api/cities/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/cities/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// GET /api/cities/{id} с не-Guid возвращает 404 (ограничение маршрута)
    /// </summary>
    [Fact]
    public async Task GetById_WhenIdIsNotGuid_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/cities/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /api/cities создаёт город и сохраняет его в БД
    /// </summary>
    [Fact]
    public async Task Create_CreatesCity()
    {
        var request = new CityCreateApiModel { Name = UniqueName("Город") };

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cities", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<CityApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.Name.Should().Be(request.Name);

        var inDb = await Context.Set<City>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.Name.Should().Be(request.Name);
    }

    /// <summary>
    /// POST /api/cities с уже существующим названием возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenNameExists_Returns406()
    {
        var seeded = await SeedCityAsync();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cities", new CityCreateApiModel { Name = seeded.Name });

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        var detail = await response.Content.ReadFromJsonAsync<ApiExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Message.Should().Contain("уже существует");
    }

    /// <summary>
    /// POST /api/cities с невалидной моделью возвращает 422 и список ошибок
    /// </summary>
    [Fact]
    public async Task Create_WhenInvalid_Returns422WithErrors()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cities", new CityCreateApiModel { Name = string.Empty });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Errors.Should().NotBeEmpty();
        detail.Errors.Should().Contain(x => x.Message.Contains("Название города обязательно"));
    }

    /// <summary>
    /// POST /api/cities с названием длиннее допустимого возвращает 422
    /// </summary>
    [Fact]
    public async Task Create_WhenNameTooLong_Returns422()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/cities", new CityCreateApiModel { Name = new string('г', 101) });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail!.Errors.Should().Contain(x => x.Message.Contains("не должно превышать"));
    }

    /// <summary>
    /// PUT /api/cities/{id} обновляет город
    /// </summary>
    [Fact]
    public async Task Update_UpdatesCity()
    {
        var seeded = await SeedCityAsync();
        var request = new CityCreateApiModel { Name = UniqueName("Новый") };

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cities/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<City>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.Name.Should().Be(request.Name);
    }

    /// <summary>
    /// PUT /api/cities/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cities/{Guid.NewGuid()}",
            new CityCreateApiModel { Name = UniqueName("Город") });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /api/cities/{id} с названием другого города возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenNameTakenByOther_Returns406()
    {
        var first = await SeedCityAsync();
        var second = await SeedCityAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cities/{second.Id}",
            new CityCreateApiModel { Name = first.Name });

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /api/cities/{id} с невалидной моделью возвращает 422
    /// </summary>
    [Fact]
    public async Task Update_WhenInvalid_Returns422()
    {
        var seeded = await SeedCityAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/cities/{seeded.Id}",
            new CityCreateApiModel { Name = string.Empty });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    /// <summary>
    /// DELETE /api/cities/{id} мягко удаляет город: строка остаётся в БД с DeletedAt, но API её не отдаёт
    /// </summary>
    [Fact]
    public async Task Delete_DeletesCity()
    {
        var seeded = await SeedCityAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/cities/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<City>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/api/cities/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /api/cities/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/cities/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}"[..30];

    private async Task<City> SeedCityAsync()
    {
        var entity = new City
        {
            Id = Guid.NewGuid(),
            Name = UniqueName("Город"),
        };
        Context.Set<City>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}