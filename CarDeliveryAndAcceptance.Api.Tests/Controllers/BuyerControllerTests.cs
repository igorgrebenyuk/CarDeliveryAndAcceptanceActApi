using System.Net;
using System.Net.Http.Json;
using CarDeliveryAndAcceptanceActApi.Models;
using CarDeliveryAndAcceptanceActApi.Models.Buyer;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using CarDeliveryAndAcceptance.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты BuyerController
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class BuyerControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BuyerControllerTests"/>
    /// </summary>
    public BuyerControllerTests(CarDeliveryAndAcceptanceFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// GET /buyers возвращает всех покупателей
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAll()
    {
        var expected = await SeedAsync();
        await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/api/buyers");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<BuyerApiModel>>();
        items.Should().NotBeNull();
        items!.Should().Contain(x => x.Id == expected.Id && x.Name == expected.Name);
    }

    /// <summary>
    /// GET /buyers/{id} возвращает покупателя
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsItem()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/api/buyers/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<BuyerApiModel>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
        item.Name.Should().Be(seeded.Name);
        item.FirstName.Should().Be(seeded.FirstName);
        item.Surname.Should().Be(seeded.Surname);
    }

    /// <summary>
    /// GET /buyers/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/buyers/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// GET /buyers/{id} с не-Guid возвращает 404 (ограничение маршрута)
    /// </summary>
    [Fact]
    public async Task GetById_WhenIdIsNotGuid_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/buyers/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /buyers создаёт покупателя и сохраняет в БД
    /// </summary>
    [Fact]
    public async Task Create_Creates()
    {
        var request = ValidRequest();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/buyers", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<BuyerApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.Name.Should().Be(request.Name);
        created.FirstName.Should().Be(request.FirstName);
        created.Surname.Should().Be(request.Surname);

        var inDb = await Context.Set<Buyer>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.Name.Should().Be(request.Name);
    }

    /// <summary>
    /// POST /buyers с уже существующим названием возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenNameExists_Returns406()
    {
        var seeded = await SeedAsync();
        var request = ValidRequest();
        request.Name = seeded.Name;

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/buyers", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        var detail = await response.Content.ReadFromJsonAsync<ApiExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Message.Should().Contain("уже существует");
    }

    /// <summary>
    /// POST /buyers с пустой моделью возвращает 422 и ошибки по всем обязательным полям
    /// </summary>
    [Fact]
    public async Task Create_WhenInvalid_Returns422WithErrors()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/buyers", new BuyerCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Errors.Should().Contain(x => x.Message.Contains("Наименование организации покупателя обязательно"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Имя покупателя обязательно"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Фамилия покупателя обязательн"));
    }

    /// <summary>
    /// POST /buyers со слишком длинным значением возвращает 422
    /// </summary>
    [Fact]
    public async Task Create_WhenFieldTooLong_Returns422()
    {
        var request = ValidRequest();
        request.Surname = new string('ф', 51);

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/buyers", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail!.Errors.Should().Contain(x => x.Message.Contains("не должн"));
    }

    /// <summary>
    /// PUT /buyers/{id} обновляет покупателя
    /// </summary>
    [Fact]
    public async Task Update_Updates()
    {
        var seeded = await SeedAsync();
        var request = ValidRequest();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/buyers/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Buyer>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.Name.Should().Be(request.Name);
        inDb.FirstName.Should().Be(request.FirstName);
        inDb.Surname.Should().Be(request.Surname);
    }

    /// <summary>
    /// PUT /buyers/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/buyers/{Guid.NewGuid()}", ValidRequest());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /buyers/{id} с названием другого покупателя возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenNameTakenByOther_Returns406()
    {
        var first = await SeedAsync();
        var second = await SeedAsync();
        var request = ValidRequest();
        request.Name = first.Name;

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/buyers/{second.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /buyers/{id} с невалидной моделью возвращает 422
    /// </summary>
    [Fact]
    public async Task Update_WhenInvalid_Returns422()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/buyers/{seeded.Id}", new BuyerCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    /// <summary>
    /// DELETE /buyers/{id} мягко удаляет: строка остаётся в БД с DeletedAt, API её не отдаёт
    /// </summary>
    [Fact]
    public async Task Delete_Deletes()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/buyers/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Buyer>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/api/buyers/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /buyers/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/buyers/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static BuyerCreateApiModel ValidRequest() => new()
    {
        Name = $"Buyer-{Guid.NewGuid():N}",
        FirstName = "Иван",
        Surname = "Иванов",
    };

    private async Task<Buyer> SeedAsync()
    {
        var entity = new Buyer
        {
            Id = Guid.NewGuid(),
            Name = $"Buyer-{Guid.NewGuid():N}",
            FirstName = "Пётр",
            Surname = "Петров",
        };
        Context.Set<Buyer>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}