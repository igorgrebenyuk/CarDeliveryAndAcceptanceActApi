using AutoMapper;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Services.Contracts.Models;
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
        CreateMap<AcceptanceAct, AcceptanceActModel>();
        CreateMap<Buyer, BuyerModel>();
        CreateMap<Car, CarModel>();
        CreateMap<City, CityModel>();
        CreateMap<Salesman, SalesmanModel>();
    }
}