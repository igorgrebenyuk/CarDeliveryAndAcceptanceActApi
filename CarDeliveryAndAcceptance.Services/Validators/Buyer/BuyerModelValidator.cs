using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using FluentValidation;

namespace CarDeliveryAndAcceptance.Services.Validators.Buyer;

/// <summary>
/// Валидатор для <see cref="BuyerModel"/>
/// </summary>
public class BuyerModelValidator : AbstractValidator<BuyerModel>
{   
    
        
    /// <summary>
    /// ctor
    /// </summary>
    public BuyerModelValidator()
    {
        Include(new BuyerCreateModelValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Покупатель обязателен");
    }
    
}