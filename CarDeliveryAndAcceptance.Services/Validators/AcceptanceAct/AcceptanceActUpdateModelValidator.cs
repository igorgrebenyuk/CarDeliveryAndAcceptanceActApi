using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.AcceptanceAct;

/// <summary>
/// Валидатор для <see cref="AcceptanceActUpdateModel"/>
/// </summary>
public class AcceptanceActUpdateModelValidator : AbstractValidator<AcceptanceActUpdateModel>
{   
    /// <summary>
    /// ctor
    /// </summary
    public AcceptanceActUpdateModelValidator()
    {
        Include(new AcceptanceActCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Идентификатор акта обязателен.");
    }
}