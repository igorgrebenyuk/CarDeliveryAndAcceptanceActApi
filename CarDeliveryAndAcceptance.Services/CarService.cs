using AutoMapper;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services;

public class CarService : ICarService
{
    private readonly IMapper mapper;
    private readonly ICarRepository carRepository;
    
    public CarService(ICarRepository carRepository, IMapper mapper)
    {
        this.carRepository = carRepository;
        this.mapper = mapper;
    }


    async Task<IReadOnlyCollection<CarModel>> ICarService.GetCarsAsync(CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarsAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<CarModel>>(result);
    }

    async Task<CarModel?> ICarService.GetCarByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByIdAsync(id, cancellationToken);
        return mapper.Map<CarModel>(result);
    }

    async Task<CarModel?> ICarService.GetCarByVinCodeAsync(string vinCode, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByVinCodeAsync(vinCode , cancellationToken);
        return mapper.Map<CarModel>(result);
    }

    async Task<CarModel?> ICarService.GetCarByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
    {
        var result = await carRepository.GetCarByVinCodeAsync(licensePlate , cancellationToken);
        return mapper.Map<CarModel>(result);
    }
}