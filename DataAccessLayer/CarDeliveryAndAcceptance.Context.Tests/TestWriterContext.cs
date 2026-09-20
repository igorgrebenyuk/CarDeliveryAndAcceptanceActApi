using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using Moq;

namespace CarDeliveryAndAcceptance.Context.Tests;

public class TestWriterContext : IDbWriterContext
{
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;
    private readonly Mock<IIdentityProvider> identityProviderMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TestWriterContext"/>
    /// </summary>
    public TestWriterContext(IWriter writer)
    {
        Writer = writer;
        dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        identityProviderMock = new Mock<IIdentityProvider>();
        identityProviderMock.Setup(x => x.Name).Returns("test@test-identity");
    }

    /// <summary>
    /// Писатель сущностей в базу данных
    /// </summary>
    public IWriter Writer { get; }

    /// <inheritdoc cref="IDateTimeProvider"/>
    public IDateTimeProvider DateTimeProvider => dateTimeProviderMock.Object;

    /// <inheritdoc cref="IIdentityProvider"/>
    public IIdentityProvider IdentityProvider => identityProviderMock.Object;
}