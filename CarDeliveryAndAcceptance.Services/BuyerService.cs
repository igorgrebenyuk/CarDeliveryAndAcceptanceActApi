using AutoMapper;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;

namespace CarDeliveryAndAcceptance.Services;

public class BuyerService : IBuyerService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IBuyerRepository buyerRepository;
    
    public BuyerService(IBuyerRepository buyerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.buyerRepository = buyerRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получение всех покупателей
    /// </summary>
    async Task<IReadOnlyCollection<BuyerModel>> IBuyerService.GetBuyersAsync(CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyersAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<BuyerModel>>(result);
    }
    
    /// <summary>
    /// Получение покупателя по ID
    /// </summary>
    async Task<BuyerModel> IBuyerService.GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyerByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<Buyer>(id);
        }
        return mapper.Map<BuyerModel>(result);
    }
    
    /// <summary>
    /// Поиск покупателя по названию компании
    /// </summary>
    async Task<BuyerModel> IBuyerService.GetBuyerByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyerByNameAsync(Name , cancellationToken);
        return mapper.Map<BuyerModel>(result);
    }

    /// <summary>
    ///Добавление покупателя
    /// </summary>
    async Task IBuyerService.CreateBuyerAsync(BuyerCreateModel buyerCreateModel, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Buyer>(buyerCreateModel);
        buyerRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Обновление покупателя
    /// </summary>
    async Task IBuyerService.UpdateBuyerAsync(BuyerModel buyerModel, CancellationToken cancellationToken)
    {
        var entity = await buyerRepository.GetBuyerByIdAsync(buyerModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Buyer>(buyerModel.Id);
        }

        mapper.Map(buyerModel, entity);
        buyerRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///Удаление покупателя
    /// </summary>
    async Task IBuyerService.DeleteBuyerAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await buyerRepository.GetBuyerByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Buyer>(id);
        }

        buyerRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}