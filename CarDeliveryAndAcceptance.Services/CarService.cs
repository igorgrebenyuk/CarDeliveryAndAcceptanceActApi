using AutoMapper;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Exceptions;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;

namespace CarDeliveryAndAcceptance.Services;

public class CarService : ICarService
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly ICarRepository carRepository;
    
    /// <summary>
    /// Получение всех автомобилей
    /// </summary>
    public CarService(ICarRepository carRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.carRepository = carRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получение автомобиля по ID
    /// </summary>
    async Task<IReadOnlyCollection<CarModel>> ICarService.GetCarsAsync(CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarsAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<CarModel>>(result);
    }
    
    /// <summary>
    /// Поиск автомобиля по VIN-коду
    /// </summary>
    async Task<CarModel?> ICarService.GetCarByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByIdAsync(id, cancellationToken);
        return mapper.Map<CarModel>(result);
    }
    
    
    /// <summary>
    /// Поиск автомобиля по VIN-коду
    /// </summary>
    async Task<CarModel?> ICarService.GetCarByVinCodeAsync(string vinCode, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByVinCodeAsync(vinCode , cancellationToken);
        return mapper.Map<CarModel>(result);
    }
    
    
    /// <summary>
    /// Поиск автомобиля по регистрационному знаку
    /// </summary>
    async Task<CarModel?> ICarService.GetCarByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByVinCodeAsync(licensePlate , cancellationToken);
        return mapper.Map<CarModel>(result);
    }
    
    /// <summary>
    ///Добавление автомобиля
    /// </summary>
    async Task ICarService.CreateCarAsync(CarCreateModel carCreateModel, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Car>(carCreateModel);
        carRepository.Add(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    ///Обновление автомобиля
    /// </summary>
    async Task ICarService.UpdateCarAsync(CarModel carModel, CancellationToken cancellationToken)
    {
        var entity = await carRepository.GetCarByIdAsync(carModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Car>(carModel.Id);
        }

        mapper.Map(carModel, entity);
        carRepository.Update(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///Удаление автомобиля
    /// </summary>
    async Task ICarService.DeleteCarAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await carRepository.GetCarByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Car>(id);
        }

        carRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}