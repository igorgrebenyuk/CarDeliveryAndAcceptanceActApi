using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.AcceptanceAct;

public class AcceptanceActUpdateModelValidator : AbstractValidator<AcceptanceActModel>
{
    AcceptanceActUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Идентификатор акта обязателен.");
    }
}