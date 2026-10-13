using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Validators.Constraints;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Buyer;

/// <summary>
/// Валидатор для <see cref="BuyerCreateModel"/>
/// </summary>
public class BuyerCreateModelValidator: AbstractValidator<BuyerCreateModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public BuyerCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Наименование организации покупателя обязательно")
            .MaximumLength(ByuerConstraints.MaxNameLength)
            .WithMessage($"Наименование организации покупателя не должно превышать {ByuerConstraints.MaxNameLength} символов");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя покупателя обязательно")
            .MaximumLength(ByuerConstraints.MaxFirstNameLength)
            .WithMessage($"Имя покупателя не должно превышать {ByuerConstraints.MaxFirstNameLength} символов");
        
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Фамилия покупателя обязательно")
            .MaximumLength(ByuerConstraints.MaxSurnameLength)
            .WithMessage($"Фамилия покупателя не должно превышать {ByuerConstraints.MaxSurnameLength} символов");
    }
}