using AutoMapper;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

namespace CarDeliveryAndAcceptance.Services.Automapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Buyer, BuyerModel>().ReverseMap();
        CreateMap<BuyerCreateModel, Buyer>();

        CreateMap<Car, CarModel>().ReverseMap();
        CreateMap<CarCreateModel, Car>();

        CreateMap<City, CityModel>().ReverseMap();
        CreateMap<CityCreateModel, City>();

        CreateMap<Salesman, SalesmanModel>().ReverseMap();
        CreateMap<SalesmanCreateModel, Salesman>();

        CreateMap<AcceptanceAct, AcceptanceActModel>();
        CreateMap<AcceptanceActCreateModel, AcceptanceAct>();
        CreateMap<AcceptanceActUpdateModel, AcceptanceAct>();
    }
}