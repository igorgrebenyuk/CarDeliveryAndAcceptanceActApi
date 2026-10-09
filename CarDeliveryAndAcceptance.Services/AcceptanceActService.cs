using AutoMapper;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

namespace CarDeliveryAndAcceptance.Services;

public class AcceptanceActService : IAcceptanceActService
{
    private readonly IMapper mapper;
    private readonly IAcceptanceActRepository acceptanceActRepository;
    
    public AcceptanceActService(IAcceptanceActRepository acceptanceActRepository, IMapper mapper)
    {
        this.acceptanceActRepository = acceptanceActRepository;
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
    
    
}