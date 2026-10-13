namespace CarDeliveryAndAcceptance.Services.Validators.Constraints;

/// <summary>
/// Константы для покупателя
/// </summary>
public class ByuerConstraints
{
    /// <summary>
    /// Максимальная длина наименования компании покупателя
    /// </summary>
    public const int MaxNameLength = 150;
    
    /// <summary>
    /// Максимальная длина имени покупателя
    /// </summary>
    public const int MaxFirstNameLength = 100;
    
    
    /// <summary>
    /// Максимальная длина фамилии покупателя
    /// </summary>
    public const int MaxSurnameLength = 50;
}