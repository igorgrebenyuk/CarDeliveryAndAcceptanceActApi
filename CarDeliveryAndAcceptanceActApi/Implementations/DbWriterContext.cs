using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

namespace CarDelivery_AcceptanceActApi.Implementations;

/// <summary>
/// Контекст записи данных: объединяет писателя, поставщик времени и поставщик идентичности
/// </summary>
/// <param name="writer">Писатель сущностей в базу данных</param>
/// <param name="dateTimeProvider">Поставщик текущей даты и времени</param>
/// <param name="identityProvider">Поставщик информации о текущем пользователе</param>
public class DbWriterContext(IWriter writer, IDateTimeProvider dateTimeProvider, IIdentityProvider identityProvider) : IDbWriterContext
{
    /// <summary>
    /// Писатель сущностей в базу данных
    /// </summary>
    public IWriter Writer => writer;
 
    /// <summary>
    /// Поставщик текущей даты и времени
    /// </summary>
    public IDateTimeProvider DateTimeProvider => dateTimeProvider;
 
    /// <summary>
    /// Поставщик информации о текущем пользователе
    /// </summary>
    public IIdentityProvider IdentityProvider => identityProvider;
}