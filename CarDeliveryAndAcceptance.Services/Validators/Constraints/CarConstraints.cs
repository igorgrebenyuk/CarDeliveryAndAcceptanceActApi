namespace CarDeliveryAndAcceptance.Services.Validators.Constraints;

/// <summary>
/// Константы для автомобиля
/// </summary>
public class CarConstraints
{
    /// <summary>
    /// Максимальная длина марки автомобиля
    /// </summary>
    public const int MaxMarkLength = 100;

    /// <summary>
    /// Максимальная длина модели автомобиля
    /// </summary>
    public const int MaxModelLength = 100;

    /// <summary>
    /// Максимальная длина номерного знака
    /// </summary>
    public const int MaxLicensePlateLength = 20;

    /// <summary>
    /// Максимальная длина VIN-кода
    /// </summary>
    public const int MaxVinCodeLength = 17;
    
    public const int MinManufactureYearLength = 1885;
    
    /// <summary>
    /// Максимальная длина номера двигателя
    /// </summary>
    public const int MaxEngineNumberLength = 50;

    /// <summary>
    /// Максимальная длина номера шасси
    /// </summary>
    public const int MaxChassisNumberLength = 50;

    /// <summary>
    /// Максимальная длина номера кузова
    /// </summary>
    public const int MaxBodyNumberLength = 50;

    /// <summary>
    /// Максимальная длина цвета автомобиля
    /// </summary>
    public const int MaxColorLength = 50;
}