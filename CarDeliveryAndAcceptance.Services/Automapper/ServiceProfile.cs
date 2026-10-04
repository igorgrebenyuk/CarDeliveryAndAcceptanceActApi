using AutoMapper;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Services.Contracts.Models;

namespace CarDeliveryAndAcceptance.Services.Automapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<AcceptanceAct, AcceptanceActModel>();
        CreateMap<Buyer, BuyerModel>();
        CreateMap<Car, CarModel>();
        CreateMap<City, CityModel>();
        CreateMap<Salesman, SalesmanModel>();
    }
}