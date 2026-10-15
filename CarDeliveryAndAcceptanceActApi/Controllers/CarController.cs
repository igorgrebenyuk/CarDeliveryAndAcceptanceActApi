using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CarDelivery_AcceptanceActApi.Controllers;

/// <summary>
/// Автомобили
/// </summary>
[ApiController]
[Route("api/cars")]
public class CarController(ICarService carService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получить все автомобили
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CarApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await carService.GetCarsAsync(ct);
        return Ok(mapper.Map<IReadOnlyCollection<CarApiModel>>(result));
    }

    /// <summary>
    /// Получить автомобиль по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<CarApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await carService.GetCarByIdAsync(id, ct);
        return Ok(mapper.Map<CarApiModel>(result));
    }

}