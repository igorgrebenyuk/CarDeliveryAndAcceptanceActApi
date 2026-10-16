using AutoMapper;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptanceActApi.Models.Buyer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarDeliveryAndAcceptanceActApi.Controllers;

/// <summary>
/// Покупатели
/// </summary>
[ApiController]
[Route("api/buyers")]
public class BuyerController : ControllerBase
{
    private readonly IBuyerService buyerService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="buyerService">Сервис работы с покупателя</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public BuyerController(IBuyerService buyerService, IMapper mapper, IValidateService validateService)
    {
        this.buyerService = buyerService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получить всех покупателей
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<BuyerApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await buyerService.GetBuyersAsync(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<BuyerApiModel>>(result));
    }

    /// <summary>
    /// Получить покупателя по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<BuyerApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([SwaggerParameter("Идентификатор покупателя")] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await buyerService.GetBuyerByIdAsync(id, cancellationToken);
        return Ok(mapper.Map<BuyerApiModel>(result));
    }

    /// <summary>
    /// Создать покупателя
    /// </summary>
    [HttpPost]
    [ProducesResponseType<BuyerApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(BuyerCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<BuyerCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);

        var result = await buyerService.CreateBuyerAsync(mapped, cancellationToken);
        return Ok(mapper.Map<BuyerApiModel>(result));
    }

    /// <summary>
    /// Обновить покупателя
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BuyerCreateApiModel createModel,
        CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<BuyerModel>(createModel);
        mapped.Id = id;
        await validateService.ValidateAsync(mapped, cancellationToken);

        await buyerService.UpdateBuyerAsync(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить покупателя
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([SwaggerParameter("Идентификатор покупателя")] Guid id,
        CancellationToken cancellationToken)
    {
        await buyerService.DeleteBuyerAsync(id, cancellationToken);
        return NoContent();
    }
}