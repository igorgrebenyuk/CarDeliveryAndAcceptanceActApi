using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.AcceptanceAct;

public class AcceptanceActCreateModelValidator : AbstractValidator<AcceptanceActCreateModel>
{
    public AcceptanceActCreateModelValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("Идентификатор города обязателен.");

        RuleFor(x => x.BuyerId)
            .NotEmpty().WithMessage("Идентификатор покупателя обязателен.");

        RuleFor(x => x.SalesmanId)
            .NotEmpty().WithMessage("Идентификатор продавца обязателен.");

        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("Идентификатор автомобиля обязателен.");
    }
}