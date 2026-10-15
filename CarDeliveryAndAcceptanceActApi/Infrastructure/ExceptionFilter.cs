using CarDelivery_AcceptanceActApi.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarDelivery_AcceptanceActApi.Infrastructure;

public class ExceptionFilter : IExceptionFilter
{
    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception as CarDeliveryAndAcceptanceException;
        if (exception == null)
        {
            return;
        }

        switch (exception)
        {
            case NotFoundException ex:
                SetDataToContext(new NotFoundObjectResult(new ApiExceptionDetail
                {
                    Message = ex.Message,
                }), context);
                break;

            case CarDeliveryAndAcceptanceInvalidOperationException ex:
                SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail { Message = ex.Message, })
                {
                    StatusCode = StatusCodes.Status406NotAcceptable,
                }, context);
                break;
            case CarDeliveryAndAcceptanceValidationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiValidationExceptionDetail { Errors = ex.Errors, })
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    },
                    context);
                break;

            default:
                SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail
                {
                    Message = exception.Message,
                }), context);
                break;
        }
    }

    /// <summary>
    /// Определяет контекст ответа
    /// </summary>
    private static void SetDataToContext(ObjectResult data, ExceptionContext context)
    {
        context.ExceptionHandled = true;
        var response = context.HttpContext.Response;
        response.StatusCode = data.StatusCode ?? StatusCodes.Status400BadRequest;
        context.Result = data;
    }
}