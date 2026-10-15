using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Salesman;

/// <summary>
/// Валидатор для <see cref="SalesmanModel"/>
/// </summary>
public class SalesmanModelValidator : AbstractValidator<SalesmanModel>
{
    public SalesmanModelValidator()
    {   
        Include(new SalesmanCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Продавец обязателен");
    }
}