using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;


public class BuyerRepository : BaseWriteRepository<Buyer>, IBuyerRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public BuyerRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех покупателей
    /// </summary>
    Task<IReadOnlyCollection<Buyer>> IBuyerRepository.GetBuyersAsync(CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<Buyer?> IBuyerRepository.GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    ///<summary>
    ///Получения продавца по названию компании
    /// </summary>>
    Task<Buyer?> IBuyerRepository.GetBuyerByNameAsync( string Name, CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .Where(x => x.Name.ToLower() == Name.ToLower())
            .FirstOrDefaultAsync(cancellationToken);
    
    /// <summary>
    /// Поиск продавца по имени и фамилии
    /// </summary>
    Task<Buyer?> IBuyerRepository.GetBuyerByFirstNameAndSurnameAsync(
        string firstName, 
        string surname, 
        CancellationToken cancellationToken)
        => reader.Read<Buyer>()
            .NotDeletedAt()
            .Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.Surname.ToLower() == surname.ToLower())
            .FirstOrDefaultAsync(cancellationToken);
}