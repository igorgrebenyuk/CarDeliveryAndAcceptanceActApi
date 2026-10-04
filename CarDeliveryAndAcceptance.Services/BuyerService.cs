using AutoMapper;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services;

public class BuyerService : IBuyerService
{
    private readonly IMapper mapper;
    private readonly IBuyerRepository buyerRepository;
    
    public BuyerService(IBuyerRepository buyerRepository, IMapper mapper)
    {
        this.buyerRepository = buyerRepository;
        this.mapper = mapper;
    }


    async Task<IReadOnlyCollection<BuyerModel>> IBuyerService.GetBuyersAsync(CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyersAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<BuyerModel>>(result);
    }

    async Task<BuyerModel?> IBuyerService.GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyerByIdAsync(id, cancellationToken);
        return mapper.Map<BuyerModel>(result);
    }

    async Task<BuyerModel?> IBuyerService.GetBuyerByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await buyerRepository.GetBuyerByNameAsync(Name , cancellationToken);
        return mapper.Map<BuyerModel>(result);
    }
}