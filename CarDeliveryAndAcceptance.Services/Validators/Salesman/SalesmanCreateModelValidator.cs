using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using CarDeliveryAndAcceptance.Services.Validators.Constraints;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Salesman;

/// <summary>
/// Валидатор для <see cref="SalesmanCreateModel"/>
/// </summary>
public class SalesmanCreateModelValidator : AbstractValidator<SalesmanCreateModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public SalesmanCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название продавца обязательно")
            .MaximumLength(SalesmanConstraints.MaxNameLength)
            .WithMessage($"Название продавца не должно превышать {SalesmanConstraints.MaxNameLength} символов");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя продавца обязательно")
            .MaximumLength(SalesmanConstraints.MaxFirstNameLength)
            .WithMessage($"Имя продавца не должно превышать {SalesmanConstraints.MaxFirstNameLength} символов");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Фамилия продавца обязательна")
            .MaximumLength(SalesmanConstraints.MaxSurnameLength)
            .WithMessage($"Фамилия продавца не должна превышать {SalesmanConstraints.MaxSurnameLength} символов");
    }
}