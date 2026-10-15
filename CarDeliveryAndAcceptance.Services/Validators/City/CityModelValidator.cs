using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.City;

/// <summary>
/// Валидатор для <see cref="CityModel"/>
/// </summary>
public class CityModelValidator : AbstractValidator<CityModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public CityModelValidator()
    {   
        Include(new CityCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Город обязателен");
    }
}