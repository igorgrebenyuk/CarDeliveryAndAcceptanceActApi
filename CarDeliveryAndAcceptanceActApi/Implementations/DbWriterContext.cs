using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;

namespace CarDelivery_AcceptanceActApi.Implementations;

/// <summary>
/// 
/// </summary>
public class DbWriterContext(IWriter writer, IDateTimeProvider dateTimeProvider, IIdentityProvider identityProvider) : IDbWriterContext
{
    /// <summary>
    /// 
    /// </summary>
    public IWriter Writer => writer;
    /// <summary>
    /// 
    /// </summary>
    public IDateTimeProvider DateTimeProvider { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public IIdentityProvider IdentityProvider { get; }
}