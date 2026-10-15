using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Car;

/// <summary>
/// Валидатор для <see cref="CarModel"/>
/// </summary>
public class CarModelValidator : AbstractValidator<CarModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public CarModelValidator()
    {   
        Include(new CarCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Автомобиль обязателен");
    }
}