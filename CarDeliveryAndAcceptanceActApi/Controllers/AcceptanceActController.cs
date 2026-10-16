using AutoMapper;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptanceActApi.Models.AcceptanceAct;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarDeliveryAndAcceptanceActApi.Controllers;

/// <summary>
/// Акты приёма-передачи автомобилей
/// </summary>
[ApiController]
[Route("api/acceptance-acts")]
public class AcceptanceActController : ControllerBase
{
    private readonly IAcceptanceActService acceptanceActService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="acceptanceActService">Сервис работы с актами</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public AcceptanceActController(IAcceptanceActService acceptanceActService, IMapper mapper,
        IValidateService validateService)
    {
        this.acceptanceActService = acceptanceActService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получить все акты (краткая информация)
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<AcceptanceActListItemApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await acceptanceActService.GetAcceptanceActsAsync(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<AcceptanceActListItemApiModel>>(result));
    }

    /// <summary>
    /// Получить акт по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<AcceptanceActApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([SwaggerParameter("Идентификатор акта")] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await acceptanceActService.GetAcceptanceActByIdAsync(id, cancellationToken);
        return Ok(mapper.Map<AcceptanceActApiModel>(result));
    }

    /// <summary>
    /// Создать акт
    /// </summary>
    [HttpPost]
    [ProducesResponseType<AcceptanceActApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(AcceptanceActCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<AcceptanceActCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);

        var result = await acceptanceActService.CreateAcceptanceActAsync(mapped, cancellationToken);
        return Ok(mapper.Map<AcceptanceActApiModel>(result));
    }

    /// <summary>
    /// Обновить акт
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AcceptanceActCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<AcceptanceActUpdateModel>(createModel);
        mapped.Id = id;
        await validateService.ValidateAsync(mapped, cancellationToken);

        await acceptanceActService.UpdateAcceptanceActAsync(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить акт
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор акта")] Guid id,
        CancellationToken cancellationToken)
    {
        await acceptanceActService.DeleteAcceptanceActAsync(id, cancellationToken);
        return NoContent();
    }
}