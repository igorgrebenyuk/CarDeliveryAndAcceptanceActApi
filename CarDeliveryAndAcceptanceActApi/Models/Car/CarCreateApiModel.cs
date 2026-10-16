namespace CarDeliveryAndAcceptanceActApi.Models.Car;

/// <summary>
/// Модель создания
/// </summary>
public class CarCreateApiModel
{
    /// <summary>
    /// Марка
    /// </summary>
    public string Mark { get; set; } = string.Empty;

    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Регистрационный знак
    /// </summary>
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// VIN-код
    /// </summary>
    public string VinCode { get; set; } = string.Empty;

    /// <summary>
    /// Год выпуска
    /// </summary>
    public int ManufactureYear { get; set; }

    /// <summary>
    /// Номер двигателя
    /// </summary>
    public string EngineNumber { get; set; } = string.Empty;

    /// <summary>
    /// Номер шасси
    /// </summary>
    public string ChassisNumber { get; set; } = string.Empty;

    /// <summary>
    /// Номер кузова
    /// </summary>
    public string BodyNumber { get; set; } = string.Empty;

    /// <summary>
    /// Цвет
    /// </summary>
    public string Color { get; set; } = string.Empty;
}