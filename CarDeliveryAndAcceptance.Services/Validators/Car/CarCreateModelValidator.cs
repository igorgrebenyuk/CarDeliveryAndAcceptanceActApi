using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Validators.Constraints;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Car;

/// <summary>
/// Валидатор для <see cref="CarCreateModel"/>
/// </summary>
public class CarCreateModelValidator : AbstractValidator<CarCreateModel>
{   
    /// <summary>
    /// ctor
    /// </summary>
    public CarCreateModelValidator()
    {
        RuleFor(x => x.Mark)
            .NotEmpty().WithMessage("Марка автомобиля обязательна")
            .MaximumLength(CarConstraints.MaxMarkLength)
            .WithMessage($"Марка автомобиля не должна превышать {CarConstraints.MaxMarkLength} символов");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Модель автомобиля обязательна")
            .MaximumLength(CarConstraints.MaxModelLength)
            .WithMessage($"Модель автомобиля не должна превышать {CarConstraints.MaxModelLength} символов");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Регистрационный знак обязателен")
            .MaximumLength(CarConstraints.MaxLicensePlateLength)
            .WithMessage($"Регистрационный знак не должен превышать {CarConstraints.MaxLicensePlateLength} символов");

        RuleFor(x => x.VinCode)
            .NotEmpty().WithMessage("VIN-код обязателен")
            .MaximumLength(CarConstraints.MaxVinCodeLength)
            .WithMessage($"VIN-код должен содержать ровно или не более {CarConstraints.MaxVinCodeLength} символов");

        RuleFor(x => x.ManufactureYear)
            .NotEmpty().WithMessage("Год выпуска обязателен")
            .GreaterThan(CarConstraints.MinManufactureYearLength).WithMessage("Год выпуска указан некорректно");

        RuleFor(x => x.EngineNumber)
            .MaximumLength(CarConstraints.MaxEngineNumberLength)
            .WithMessage($"Номер двигателя не должен превышать {CarConstraints.MaxEngineNumberLength} символов");

        RuleFor(x => x.ChassisNumber)
            .MaximumLength(CarConstraints.MaxChassisNumberLength)
            .WithMessage($"Номер шасси не должен превышать {CarConstraints.MaxChassisNumberLength} символов");

        RuleFor(x => x.BodyNumber)
            .MaximumLength(CarConstraints.MaxBodyNumberLength)
            .WithMessage($"Номер кузова не должен превышать {CarConstraints.MaxBodyNumberLength} символов");

        RuleFor(x => x.Color)
            .MaximumLength(CarConstraints.MaxColorLength)
            .WithMessage($"Цвет не должен превышать {CarConstraints.MaxColorLength} символов");
    }
}