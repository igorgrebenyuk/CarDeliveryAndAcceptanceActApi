using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.AcceptanceAct;

/// <summary>
/// Валидатор для <see cref="AcceptanceActModel"/>
/// </summary>
public class AcceptanceActModelValidator : AbstractValidator<AcceptanceActModel>
{
    /// <summary>
    /// ctor
    /// </summary>
    public AcceptanceActModelValidator()
    {
        Include(new AcceptanceActCreateModelValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Идентификатор акта обязателен.");

        RuleFor(x => x.DateOfCreation)
            .NotEmpty().WithMessage("Дата создания обязательна.");
    }
}