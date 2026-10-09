using AutoMapper;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

namespace CarDeliveryAndAcceptance.Services;

public class SalesmanService : ISalesmanService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly ISalesmanRepository salesmanRepository;
    
    public SalesmanService(ISalesmanRepository salesmanRepository,IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.salesmanRepository = salesmanRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    /// <summary>
    /// Получение всех продавцов
    /// </summary>
    async Task<IReadOnlyCollection<SalesmanModel>> ISalesmanService.GetSalesmanAsync(CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<SalesmanModel>>(result);
    }
    
    /// <summary>
    /// Получение продавца по ID
    /// </summary>
    async Task<SalesmanModel> ISalesmanService.GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanByIdAsync(id, cancellationToken);
        return mapper.Map<SalesmanModel>(result);
    }
    
    /// <summary>
    /// Поиск продавца по Названию компании
    /// </summary>
    async Task<SalesmanModel> ISalesmanService.GetSalesmanByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanByNameAsync(Name , cancellationToken);
        return mapper.Map<SalesmanModel>(result);
    }
    
    /// <summary>
    ///Добавление продавца
    /// </summary>
    async Task ISalesmanService.CreateSalesmanAsync(SalesmanCreateModel salesmanCreateModel, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Salesman>(salesmanCreateModel);
        salesmanRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Обновление продавца
    /// </summary>
    async Task ISalesmanService.UpdateSalesmanAsync(SalesmanModel salesmanModel, CancellationToken cancellationToken)
    {
        var entity = await salesmanRepository.GetSalesmanByIdAsync(salesmanModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Salesman>(salesmanModel.Id);
        }

        mapper.Map(salesmanModel, entity);
        salesmanRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Удаление продавца
    /// </summary>
    async Task ISalesmanService.DeleteSalesmanAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await salesmanRepository.GetSalesmanByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Salesman>(id);
        }

        salesmanRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}