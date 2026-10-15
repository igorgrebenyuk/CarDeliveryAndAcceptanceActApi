using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.Salesman;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarDelivery_AcceptanceActApi.Controllers;

/// <summary>
/// Продавцы
/// </summary>
[ApiController]
[Route("api/salesmen")]
public class SalesmanController : ControllerBase
{
    private readonly ISalesmanService salesmanService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="salesmanService">Сервис работы с продавца</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public SalesmanController(ISalesmanService salesmanService, IMapper mapper, IValidateService validateService)
    {
        this.salesmanService = salesmanService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получить всех продавцов
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<SalesmanApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await salesmanService.GetSalesmanAsync(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<SalesmanApiModel>>(result));
    }

    /// <summary>
    /// Получить продавца по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<SalesmanApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([SwaggerParameter("Идентификатор продавца")] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await salesmanService.GetSalesmanByIdAsync(id, cancellationToken);
        return Ok(mapper.Map<SalesmanApiModel>(result));
    }

    /// <summary>
    /// Создать продавца
    /// </summary>
    [HttpPost]
    [ProducesResponseType<SalesmanApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(SalesmanCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<SalesmanCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);

        var result = await salesmanService.CreateSalesmanAsync(mapped, cancellationToken);
        return Ok(mapper.Map<SalesmanApiModel>(result));
    }

    /// <summary>
    /// Обновить продавца
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SalesmanCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<SalesmanModel>(createModel);
        mapped.Id = id;
        await validateService.ValidateAsync(mapped, cancellationToken);

        await salesmanService.UpdateSalesmanAsync(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить продавца
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор продавца")] Guid id,
        CancellationToken cancellationToken)
    {
        await salesmanService.DeleteSalesmanAsync(id, cancellationToken);
        return NoContent();
    }
}