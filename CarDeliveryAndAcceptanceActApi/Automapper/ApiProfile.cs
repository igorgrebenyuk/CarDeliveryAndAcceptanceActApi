using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;

namespace CarDelivery_AcceptanceActApi.Api.Automapper;

/// <summary>
/// Профиль маппера API: преобразование между API-моделями и моделями сервисов
/// </summary>
public class ApiProfile : Profile
{
    /// <summary>
    /// ctor
    /// </summary>
    public ApiProfile()
    {
        CreateMapForCar();
    }


    private void CreateMapForCar()
        {
            CreateMap<CarModel, CarApiModel>();
            CreateMap<CarCreateApiModel, CarCreateModel>();
            CreateMap<CarCreateApiModel, CarModel>();
        }
    

}