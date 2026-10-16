namespace CarDeliveryAndAcceptance.Services.Contracts.Exceptions;

/// <summary>
/// Базовое исключение приложения приёма-передачи автомобилей
/// </summary>
public class CarDeliveryAndAcceptanceException : Exception
{   
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarDeliveryAndAcceptanceException"/>
    /// </summary>
    public CarDeliveryAndAcceptanceException()
    {

    }
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CarDeliveryAndAcceptanceException"/>
    /// </summary>
    public CarDeliveryAndAcceptanceException(string message) : base(message)
    {

    }
}
