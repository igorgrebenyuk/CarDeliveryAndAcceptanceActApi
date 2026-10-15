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
    async Task<CityModel> ICityService.GetCityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<City>(id);
        }
        return mapper.Map<CityModel>(result);
    }
    
    
    /// <summary>
    /// Поиск города по названию
    /// </summary>
    async Task<CityModel> ICityService.GetCityByNameAsync(string Name, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetCityByNameAsync(Name , cancellationToken);
        
        return mapper.Map<CityModel>(result);
    }
    
    /// <summary>
    ///Добавление города
    /// </summary>
    async Task<CityModel> ICityService.CreateCityAsync(CityCreateModel cityCreateModel, CancellationToken cancellationToken)
    {
        var existing = await cityRepository.GetCityByNameAsync(cityCreateModel.Name, cancellationToken);
        if (existing is not null)
        {
            throw new CarDeliveryAndAcceptanceInvalidOperationException("Город с таким названием уже существует");
        }

        var entity = mapper.Map<City>(cityCreateModel);
        cityRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<CityModel>(entity);
    }
    
    /// <summary>
    ///Обновление города
    /// </summary>
    async Task ICityService.UpdateCityAsync(CityModel cityModel, CancellationToken cancellationToken)
    {
        var entity = await cityRepository.GetCityByIdAsync(cityModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<City>(cityModel.Id);
        }

        var existing = await cityRepository.GetCityByNameAsync(cityModel.Name, cancellationToken);
        if (existing is not null && existing.Id != cityModel.Id)
        {
            throw new CarDeliveryAndAcceptanceInvalidOperationException("Город с таким названием уже существует");
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