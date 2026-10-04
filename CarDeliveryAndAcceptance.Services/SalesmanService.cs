using AutoMapper;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services;

public class SalesmanService : ISalesmanService
{
    private readonly IMapper mapper;
    private readonly ISalesmanService salesmanRepository;
    
    public SalesmanService(ISalesmanService salesmanRepository, IMapper mapper)
    {
        this.salesmanRepository = salesmanRepository;
        this.mapper = mapper;
    }
    
    async Task<IReadOnlyCollection<SalesmanModel>> ISalesmanService.GetSalesmanAsync(CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<SalesmanModel>>(result);
    }

    async Task<SalesmanModel?> ISalesmanService.GetSalesmanByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanByIdAsync(id, cancellationToken);
        return mapper.Map<SalesmanModel>(result);
    }

    async Task<SalesmanModel?> ISalesmanService.GetSalesmanByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await salesmanRepository.GetSalesmanByNameAsync(Name , cancellationToken);
        return mapper.Map<SalesmanModel>(result);
    }
}