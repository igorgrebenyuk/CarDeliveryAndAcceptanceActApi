using AutoMapper;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services;

public class CityService : ICityService
{
    private readonly IMapper mapper;
    private readonly ICityRepository cityRepository;
    
    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        this.cityRepository = cityRepository;
        this.mapper = mapper;
    }

    async Task<IReadOnlyCollection<CityModel>> ICityService.GetCitiesAsync(CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCitiesAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<CityModel>>(result);
    }

    async Task<CityModel?> ICityService.GetCityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByIdAsync(id, cancellationToken);
        return mapper.Map<CityModel>(result);
    }

    async Task<CityModel?> ICityService.GetCityByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByNameAsync(Name , cancellationToken);
        return mapper.Map<CityModel>(result);
    }
}