using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Validators.Constraints;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.City;

/// <summary>
/// Валидатор для <see cref="CityCreateModel"/>
/// </summary>
public class CityCreateModelValidator : AbstractValidator<CityCreateModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public CityCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название города обязательно")
            .MaximumLength(CityConstraints.MaxNameLength)
            .WithMessage($"Название города не должно превышать {CityConstraints.MaxNameLength} символов");
    }
}