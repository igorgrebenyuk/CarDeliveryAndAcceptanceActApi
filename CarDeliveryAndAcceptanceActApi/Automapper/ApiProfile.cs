using AutoMapper;
using CarDelivery_AcceptanceActApi.Models.AcceptanceAct;
using CarDelivery_AcceptanceActApi.Models.Buyer;
using CarDelivery_AcceptanceActApi.Models.Car;
using CarDelivery_AcceptanceActApi.Models.City;
using CarDelivery_AcceptanceActApi.Models.Salesman;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

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
        CreateMapForCity();
        CreateMapForBuyer();
        CreateMapForSalesman();
        CreateMapForCar();
        CreateMapForAcceptanceAct();
    }
 
    private void CreateMapForCity()
    {
        CreateMap<CityModel, CityApiModel>();
        CreateMap<CityCreateApiModel, CityCreateModel>();
        CreateMap<CityCreateApiModel, CityModel>(); // для PUT: Id подставляется в контроллере
    }
 
    private void CreateMapForBuyer()
    {
        CreateMap<BuyerModel, BuyerApiModel>();
        CreateMap<BuyerCreateApiModel, BuyerCreateModel>();
        CreateMap<BuyerCreateApiModel, BuyerModel>();
    }
 
    private void CreateMapForSalesman()
    {
        CreateMap<SalesmanModel, SalesmanApiModel>();
        CreateMap<SalesmanCreateApiModel, SalesmanCreateModel>();
        CreateMap<SalesmanCreateApiModel, SalesmanModel>();
    }
 
    private void CreateMapForCar()
    {
        CreateMap<CarModel, CarApiModel>();
        CreateMap<CarCreateApiModel, CarCreateModel>();
        CreateMap<CarCreateApiModel, CarModel>();
    }
 
    private void CreateMapForAcceptanceAct()
    {
        // Вложенные City/Buyer/Salesman/Car маппятся автоматически маппами выше
        CreateMap<AcceptanceActModel, AcceptanceActApiModel>();
        CreateMap<AcceptanceActCreateApiModel, AcceptanceActCreateModel>();
        CreateMap<AcceptanceActCreateApiModel, AcceptanceActUpdateModel>(); // для PUT: Id из маршрута
    }
}