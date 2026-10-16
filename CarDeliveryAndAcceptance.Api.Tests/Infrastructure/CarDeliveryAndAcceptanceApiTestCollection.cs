using Xunit;

namespace CarDeliveryAndAcceptance.Api.Tests.Infrastructure;

/// <summary>
/// Коллекция для интеграционных тестов.
/// Все тесты коллекции используют одну фикстуру <see cref="CarDeliveryAndAcceptanceFixture"/>.
/// </summary>
[CollectionDefinition(nameof(CarDeliveryAndAcceptanceApiTestCollection))]
public class CarDeliveryAndAcceptanceApiTestCollection: ICollectionFixture<CarDeliveryAndAcceptanceFixture>;
