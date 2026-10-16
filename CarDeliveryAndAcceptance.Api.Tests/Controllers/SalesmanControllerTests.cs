using System.Net;
using System.Net.Http.Json;
using CarDelivery_AcceptanceActApi.Models;
using CarDelivery_AcceptanceActApi.Models.Salesman;
using CarDeliveryAndAcceptance.Api.Tests.Infrastructure;
using CarDeliveryAndAcceptance.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Controllers;

/// <summary>
/// Интеграционные тесты SalesmanController
/// </summary>
[Collection(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class SalesmanControllerTests : IntegrationTestBase
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SalesmanControllerTests"/>
    /// </summary>
    public SalesmanControllerTests(CarDeliveryAndAcceptanceFixture fixture) : base(fixture)
    {
    }

    /// <summary>
    /// GET /salesmen возвращает всех продавцов
    /// </summary>
    [Fact]
    public async Task Get_ReturnsAll()
    {
        var expected = await SeedAsync();
        await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync("/api/salesmen");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<SalesmanApiModel>>();
        items.Should().NotBeNull();
        items!.Should().Contain(x => x.Id == expected.Id && x.Name == expected.Name);
    }

    /// <summary>
    /// GET /salesmen/{id} возвращает продавца
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsItem()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.GetAsync($"/api/salesmen/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<SalesmanApiModel>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
        item.Name.Should().Be(seeded.Name);
        item.FirstName.Should().Be(seeded.FirstName);
        item.Surname.Should().Be(seeded.Surname);
    }

    /// <summary>
    /// GET /salesmen/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task GetById_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/salesmen/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// GET /salesmen/{id} с не-Guid возвращает 404 (ограничение маршрута)
    /// </summary>
    [Fact]
    public async Task GetById_WhenIdIsNotGuid_Returns404()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/salesmen/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// POST /salesmen создаёт продавца и сохраняет в БД
    /// </summary>
    [Fact]
    public async Task Create_Creates()
    {
        var request = ValidRequest();

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/salesmen", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await response.Content.ReadFromJsonAsync<SalesmanApiModel>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);
        created.Name.Should().Be(request.Name);
        created.FirstName.Should().Be(request.FirstName);
        created.Surname.Should().Be(request.Surname);

        var inDb = await Context.Set<Salesman>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == created.Id);
        inDb.Should().NotBeNull();
        inDb!.Name.Should().Be(request.Name);
    }

    /// <summary>
    /// POST /salesmen с уже существующим названием возвращает 406
    /// </summary>
    [Fact]
    public async Task Create_WhenNameExists_Returns406()
    {
        var seeded = await SeedAsync();
        var request = ValidRequest();
        request.Name = seeded.Name;

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/salesmen", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        var detail = await response.Content.ReadFromJsonAsync<ApiExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Message.Should().Contain("уже существует");
    }

    /// <summary>
    /// POST /salesmen с пустой моделью возвращает 422 и ошибки по всем обязательным полям
    /// </summary>
    [Fact]
    public async Task Create_WhenInvalid_Returns422WithErrors()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/salesmen", new SalesmanCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail.Should().NotBeNull();
        detail!.Errors.Should().Contain(x => x.Message.Contains("Название продавца обязательно"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Имя продавца обязательно"));
        detail.Errors.Should().Contain(x => x.Message.Contains("Фамилия продавца обязательн"));
    }

    /// <summary>
    /// POST /salesmen со слишком длинным значением возвращает 422
    /// </summary>
    [Fact]
    public async Task Create_WhenFieldTooLong_Returns422()
    {
        var request = ValidRequest();
        request.Surname = new string('ф', 51);

        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/salesmen", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var detail = await response.Content.ReadFromJsonAsync<ApiValidationExceptionDetail>();
        detail!.Errors.Should().Contain(x => x.Message.Contains("не должн"));
    }

    /// <summary>
    /// PUT /salesmen/{id} обновляет продавца
    /// </summary>
    [Fact]
    public async Task Update_Updates()
    {
        var seeded = await SeedAsync();
        var request = ValidRequest();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/salesmen/{seeded.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Salesman>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.Name.Should().Be(request.Name);
        inDb.FirstName.Should().Be(request.FirstName);
        inDb.Surname.Should().Be(request.Surname);
    }

    /// <summary>
    /// PUT /salesmen/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Update_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/salesmen/{Guid.NewGuid()}", ValidRequest());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// PUT /salesmen/{id} с названием другого продавца возвращает 406
    /// </summary>
    [Fact]
    public async Task Update_WhenNameTakenByOther_Returns406()
    {
        var first = await SeedAsync();
        var second = await SeedAsync();
        var request = ValidRequest();
        request.Name = first.Name;

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/salesmen/{second.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    /// <summary>
    /// PUT /salesmen/{id} с невалидной моделью возвращает 422
    /// </summary>
    [Fact]
    public async Task Update_WhenInvalid_Returns422()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"/api/salesmen/{seeded.Id}", new SalesmanCreateApiModel());

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    /// <summary>
    /// DELETE /salesmen/{id} мягко удаляет: строка остаётся в БД с DeletedAt, API её не отдаёт
    /// </summary>
    [Fact]
    public async Task Delete_Deletes()
    {
        var seeded = await SeedAsync();

        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/salesmen/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var inDb = await Context.Set<Salesman>().AsNoTracking().SingleAsync(x => x.Id == seeded.Id);
        inDb.DeletedAt.Should().NotBeNull();

        var getResponse = await client.GetAsync($"/api/salesmen/{seeded.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// DELETE /salesmen/{id} с несуществующим id возвращает 404
    /// </summary>
    [Fact]
    public async Task Delete_WhenNotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/salesmen/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static SalesmanCreateApiModel ValidRequest() => new()
    {
        Name = $"Salesman-{Guid.NewGuid():N}",
        FirstName = "Иван",
        Surname = "Иванов",
    };

    private async Task<Salesman> SeedAsync()
    {
        var entity = new Salesman
        {
            Id = Guid.NewGuid(),
            Name = $"Salesman-{Guid.NewGuid():N}",
            FirstName = "Пётр",
            Surname = "Петров",
        };
        Context.Set<Salesman>().Add(entity);
        await UnitOfWork.SaveChangesAsync();
        return entity;
    }
}