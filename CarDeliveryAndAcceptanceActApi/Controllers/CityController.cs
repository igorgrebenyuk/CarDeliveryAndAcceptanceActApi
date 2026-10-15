using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.City;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarDelivery_AcceptanceActApi.Controllers;

/// <summary>
/// Города
/// </summary>
[ApiController]
[Route("api/cities")]
public class CityController : ControllerBase
{
    private readonly ICityService cityService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="cityService">Сервис работы с города</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public CityController(ICityService cityService, IMapper mapper, IValidateService validateService)
    {
        this.cityService = cityService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получить все города
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CityApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await cityService.GetCitiesAsync(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<CityApiModel>>(result));
    }

    /// <summary>
    /// Получить город по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<CityApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([SwaggerParameter("Идентификатор города")] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await cityService.GetCityByIdAsync(id, cancellationToken);
        return Ok(mapper.Map<CityApiModel>(result));
    }

    /// <summary>
    /// Создать город
    /// </summary>
    [HttpPost]
    [ProducesResponseType<CityApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(CityCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CityCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);

        var result = await cityService.CreateCityAsync(mapped, cancellationToken);
        return Ok(mapper.Map<CityApiModel>(result));
    }

    /// <summary>
    /// Обновить город
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CityCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CityModel>(createModel);
        mapped.Id = id;
        await validateService.ValidateAsync(mapped, cancellationToken);

        await cityService.UpdateCityAsync(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить город
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор города")] Guid id,
        CancellationToken cancellationToken)
    {
        await cityService.DeleteCityAsync(id, cancellationToken);
        return NoContent();
    }
}