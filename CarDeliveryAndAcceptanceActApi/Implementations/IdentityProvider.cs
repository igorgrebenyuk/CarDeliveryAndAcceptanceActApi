using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptanceActApi.Implementations;

/// <summary>
/// Реализация <see cref="IIdentityProvider"/>
/// </summary>
public class IdentityProvider : IIdentityProvider
{   
    /// <summary>
    /// Имя пользователя, от лица которого выполняются операции
    /// </summary>
    public string Name => "Admin";
}