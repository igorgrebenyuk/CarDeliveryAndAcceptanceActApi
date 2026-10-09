using AutoMapper;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;

namespace CarDeliveryAndAcceptance.Services;

public class CityService : ICityService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly ICityRepository cityRepository;
    
    public CityService(ICityRepository cityRepository,IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.cityRepository = cityRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    /// <summary>
    /// Получение всех городов
    /// </summary>
    async Task<IReadOnlyCollection<CityModel>> ICityService.GetCitiesAsync(CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCitiesAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<CityModel>>(result);
    }
    
    
    /// <summary>
    /// Получение города по ID
    /// </summary>
    async Task<CityModel?> ICityService.GetCityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByIdAsync(id, cancellationToken);
        return mapper.Map<CityModel>(result);
    }
    
    
    /// <summary>
    /// Поиск города по названию
    /// </summary>
    async Task<CityModel?> ICityService.GetCityByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByNameAsync(Name , cancellationToken);
        return mapper.Map<CityModel>(result);
    }
    
    /// <summary>
    ///Добавление города
    /// </summary>
    async Task ICityService.CreateCityAsync(CityCreateModel cityCreateModel, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<City>(cityCreateModel);
        cityRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Обновление города
    /// </summary>
    async Task ICityService.UpdateCityAsync(CityModel cityModel, CancellationToken cancellationToken)
    {
        var entity = await cityRepository.GetCityByIdAsync(cityModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Car>(cityModel.Id);
        }

        mapper.Map(cityModel, entity);
        cityRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Удаление города
    /// </summary>
    async Task ICityService.DeleteCityAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await cityRepository.GetCityByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<City>(id);
        }

        cityRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}