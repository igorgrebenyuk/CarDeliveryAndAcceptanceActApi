using AutoMapper;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

namespace CarDeliveryAndAcceptance.Services;

public class AcceptanceActService : IAcceptanceActService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IAcceptanceActRepository acceptanceActRepository;
    private readonly ICarRepository carRepository;
    private readonly IBuyerRepository buyerRepository;
    private readonly ISalesmanRepository salesmanRepository;
    private readonly ICityRepository cityRepository;
    
    public AcceptanceActService(
        IAcceptanceActRepository acceptanceActRepository,
        ICarRepository carRepository,
        IBuyerRepository buyerRepository,
        ISalesmanRepository salesmanRepository,
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.acceptanceActRepository = acceptanceActRepository;
        this.carRepository = carRepository;
        this.buyerRepository = buyerRepository;
        this.salesmanRepository = salesmanRepository;
        this.cityRepository = cityRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    async Task<IReadOnlyCollection<AcceptanceActModel>> IAcceptanceActService.GetAcceptanceActsAsync(CancellationToken cancellationToken)
    {
        var result = await acceptanceActRepository.GetAcceptanceActsAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<AcceptanceActModel>>(result);
    }

    async Task<AcceptanceActModel> IAcceptanceActService.GetAcceptanceActByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await acceptanceActRepository.GetAcceptanceActByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<AcceptanceAct>(id);
        }
        return mapper.Map<AcceptanceActModel>(result);
    }

    async Task<AcceptanceActModel> IAcceptanceActService.CreateAcceptanceActAsync(AcceptanceActCreateModel acceptanceActCreateModel, CancellationToken cancellationToken)
    {
        await EnsureRelatedEntitiesExistAsync(
            acceptanceActCreateModel.CarId, acceptanceActCreateModel.BuyerId, acceptanceActCreateModel.SalesmanId, acceptanceActCreateModel.CityId, cancellationToken);

        var existing = await acceptanceActRepository.GetAcceptanceActByParticipantsAsync(
            acceptanceActCreateModel.CarId, acceptanceActCreateModel.BuyerId, acceptanceActCreateModel.SalesmanId, cancellationToken);
        if (existing is not null)
        {
            throw new CarDeliveryAndAcceptanceInvalidOperationException("Акт с таким автомобилем, покупателем и продавцом уже существует");
        }

        var entity = mapper.Map<AcceptanceAct>(acceptanceActCreateModel);
        acceptanceActRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Перечитываем акт, чтобы в ответе были вложенные город, покупатель, продавец и автомобиль
        var created = await acceptanceActRepository.GetAcceptanceActByIdAsync(entity.Id, cancellationToken);
        return mapper.Map<AcceptanceActModel>(created);
    }

    async Task IAcceptanceActService.UpdateAcceptanceActAsync(AcceptanceActUpdateModel acceptanceActModel, CancellationToken cancellationToken)
    {
        
        var entity = await acceptanceActRepository.GetAcceptanceActByIdAsync(acceptanceActModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<AcceptanceAct>(acceptanceActModel.Id);
        }
        
        await EnsureRelatedEntitiesExistAsync(
            acceptanceActModel.CarId, acceptanceActModel.BuyerId, acceptanceActModel.SalesmanId, acceptanceActModel.CityId, cancellationToken);

        var existing = await acceptanceActRepository.GetAcceptanceActByParticipantsAsync(
            acceptanceActModel.CarId, acceptanceActModel.BuyerId, acceptanceActModel.SalesmanId, cancellationToken);
        if (existing is not null && existing.Id != acceptanceActModel.Id)
        {
            throw new CarDeliveryAndAcceptanceInvalidOperationException("Акт с таким автомобилем, покупателем и продавцом уже существует");
        }

        mapper.Map(acceptanceActModel, entity);

        entity.City = null!;
        entity.Buyer = null!;
        entity.Salesman = null!;
        entity.Car = null!;

        acceptanceActRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IAcceptanceActService.DeleteAcceptanceActAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await acceptanceActRepository.GetAcceptanceActByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<AcceptanceAct>(id);
        }

        acceptanceActRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    private async Task EnsureRelatedEntitiesExistAsync(
        Guid carId, Guid buyerId, Guid salesmanId, Guid cityId, CancellationToken ct)
    {
        if (await carRepository.GetCarByIdAsync(carId, ct) is null)
            throw new EntityNotFoundException<Car>(carId);

        if (await buyerRepository.GetBuyerByIdAsync(buyerId, ct) is null)
            throw new EntityNotFoundException<Buyer>(buyerId);

        if (await salesmanRepository.GetSalesmanByIdAsync(salesmanId, ct) is null)
            throw new EntityNotFoundException<Salesman>(salesmanId);

        if (await cityRepository.GetCityByIdAsync(cityId, ct) is null)
            throw new EntityNotFoundException<City>(cityId);
    }
}