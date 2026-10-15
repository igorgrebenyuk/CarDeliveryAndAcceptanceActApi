using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarDelivery_AcceptanceActApi.Controllers;

/// <summary>
/// Автомобили
/// </summary>
[ApiController]
[Route("api/cars")]
public class CarController : ControllerBase
{
    private readonly ICarService carService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="carService">Сервис работы с автомобиля</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public CarController(ICarService carService, IMapper mapper, IValidateService validateService)
    {
        this.carService = carService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получить все автомобили
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CarApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await carService.GetCarsAsync(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<CarApiModel>>(result));
    }

    /// <summary>
    /// Получить автомобиль по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<CarApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([SwaggerParameter("Идентификатор автомобиля")] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await carService.GetCarByIdAsync(id, cancellationToken);
        return Ok(mapper.Map<CarApiModel>(result));
    }

    /// <summary>
    /// Создать автомобиль
    /// </summary>
    [HttpPost]
    [ProducesResponseType<CarApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(CarCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CarCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);

        var result = await carService.CreateCarAsync(mapped, cancellationToken);
        return Ok(mapper.Map<CarApiModel>(result));
    }

    /// <summary>
    /// Обновить автомобиль
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CarCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CarModel>(createModel);
        mapped.Id = id;
        await validateService.ValidateAsync(mapped, cancellationToken);

        await carService.UpdateCarAsync(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить автомобиль
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор автомобиля")] Guid id,
        CancellationToken cancellationToken)
    {
        await carService.DeleteCarAsync(id, cancellationToken);
        return NoContent();
    }
}