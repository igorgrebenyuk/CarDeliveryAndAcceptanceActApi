using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.AcceptanceAct;

public class AcceptanceActUpdateModelValidator : AbstractValidator<AcceptanceActUpdateModel>
{
    public AcceptanceActUpdateModelValidator()
    {
        Include(new AcceptanceActCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Идентификатор акта обязателен.");
    }
}