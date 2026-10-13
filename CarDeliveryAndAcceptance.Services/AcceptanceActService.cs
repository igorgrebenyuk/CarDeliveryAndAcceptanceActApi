using AutoMapper;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

namespace CarDeliveryAndAcceptance.Services;

public class AcceptanceActService : IAcceptanceActService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IAcceptanceActRepository acceptanceActRepository;
    
    public AcceptanceActService(IAcceptanceActRepository acceptanceActRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.acceptanceActRepository = acceptanceActRepository;
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
        return mapper.Map<AcceptanceActModel>(result);
    }

    async Task IAcceptanceActService.CreateAcceptanceActAsync(AcceptanceActCreateModel acceptanceActCreateModel, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AcceptanceAct>(acceptanceActCreateModel);
        acceptanceActRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task IAcceptanceActService.UpdateAcceptanceActAsync(AcceptanceActModel acceptanceActModel, CancellationToken cancellationToken)
    {
        var entity = await acceptanceActRepository.GetAcceptanceActByIdAsync(acceptanceActModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<AcceptanceAct>(acceptanceActModel.Id);
        }

        mapper.Map(acceptanceActModel, entity);
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
}