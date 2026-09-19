using CarDeliveryAndAcceptance.Context.Repositories;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarDeliveryAndAcceptance.Repositories;


public class SalesmanRepository : BaseWriteRepository<Salesman>, ISalesmanRepository
{
    private readonly IReader reader;

    /// <summary>
    /// ctor.
    /// </summary>
    public SalesmanRepository(IDbWriterContext writerContext, IReader reader)
        : base(writerContext)
    {
        this.reader = reader;
    }

    /// <summary>
    /// Получение всех продавцов
    /// </summary>
    Task<IReadOnlyCollection<Salesman>> ISalesmanRepository.GetSalesmanAsync(CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToReadOnlyCollectionAsync(cancellationToken);

    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    Task<Salesman?> ISalesmanRepository.GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    ///<summary>
    ///Получения продавца по названию компании
    /// </summary>>
    Task<Salesman?> ISalesmanRepository.GetSalesmanByNameAsync( string Name, CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .Where(x => x.Name.ToLower() == Name.ToLower())
            .FirstOrDefaultAsync(cancellationToken);
    
    /// <summary>
    /// Поиск продавца по имени и фамилии
    /// </summary>
    Task<Salesman?> ISalesmanRepository.GetSalesmanByFirstNameAndSurnameAsync(
        string firstName, 
        string surname, 
        CancellationToken cancellationToken)
        => reader.Read<Salesman>()
            .NotDeletedAt()
            .Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.Surname.ToLower() == surname.ToLower())
            .FirstOrDefaultAsync(cancellationToken);
}