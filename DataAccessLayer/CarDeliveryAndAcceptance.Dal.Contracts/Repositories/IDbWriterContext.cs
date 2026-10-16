using CarDeliveryAndAcceptance.Common;

namespace CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

/// <summary>
/// Контекст записи: объединяет писателя сущностей и провайдеры времени и пользователя для аудита
/// </summary>
public interface IDbWriterContext
{
    /// <inheritdoc cref="IWriter"/>
    IWriter Writer { get; }

    /// <inheritdoc cref="IDateTimeProvider"/>
    IDateTimeProvider DateTimeProvider { get; }

    /// <inheritdoc cref="IIdentityProvider"/>
    IIdentityProvider IdentityProvider { get; }
}