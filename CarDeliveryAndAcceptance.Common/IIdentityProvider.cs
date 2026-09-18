namespace CarDeliveryAndAcceptance.Common;

/// <summary>
/// Базовая функциональность идентификации пользователя
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    /// Возвращает имя текущего пользователя
    /// </summary>
    string Name { get; }
}