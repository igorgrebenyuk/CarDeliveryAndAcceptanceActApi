namespace CarDeliveryAndAcceptance.Common;

/// <summary>
/// Модель элемента невалидности
/// </summary>
public class InvalidateItemModel
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InvalidateItemModel"/>
    /// </summary>
    public InvalidateItemModel(string field, string message)
    {
        Field = field;
        Message = message;
    }

    /// <summary>
    /// Имя инвалидного поля
    /// </summary>
    /// <remarks>Если пустое, значит инвалидация относится ко всей моделе</remarks>
    public string Field { get; }

    /// <summary>
    /// Сообщение инвалидации
    /// </summary>
    public string Message { get; }
}