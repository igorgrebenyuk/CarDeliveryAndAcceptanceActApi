using AutoMapper;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;
using CarDeliveryAndAcceptanceActApi.Models.AcceptanceAct;
using CarDeliveryAndAcceptanceActApi.Models.Buyer;
using CarDeliveryAndAcceptanceActApi.Models.Car;
using CarDeliveryAndAcceptanceActApi.Models.City;
using CarDeliveryAndAcceptanceActApi.Models.Salesman;

namespace CarDeliveryAndAcceptanceActApi.Api.Automapper;

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
        CreateMap<CityCreateApiModel, CityModel>(); 
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
        CreateMap<AcceptanceActModel, AcceptanceActApiModel>();
        CreateMap<AcceptanceActListItemModel, AcceptanceActListItemApiModel>();
        CreateMap<AcceptanceActCreateApiModel, AcceptanceActCreateModel>();
        CreateMap<AcceptanceActCreateApiModel, AcceptanceActUpdateModel>();
    }
}