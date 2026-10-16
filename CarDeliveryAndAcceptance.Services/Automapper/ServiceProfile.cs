using AutoMapper;
using CarDeliveryAndAcceptance.Entities;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Buyer;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Car;
using CarDeliveryAndAcceptance.Services.Contracts.Models.City;
using CarDeliveryAndAcceptance.Services.Contracts.Models.Salesman;

namespace CarDeliveryAndAcceptance.Services.Automapper;

/// <summary>
/// Профиль маппера сервисов
/// </summary>
public class ServiceProfile : Profile
{
    /// <summary>
    /// ctor
    /// </summary>
    public ServiceProfile()
    {
        CreateMapForBuyer();
        CreateMapForCar();
        CreateMapForCity();
        CreateMapForSalesman();
        CreateMapForAcceptanceAct();
    }

    private void CreateMapForBuyer()
    {
        CreateMap<Buyer, BuyerModel>().ReverseMap();
        CreateMap<BuyerCreateModel, Buyer>();
    }

    private void CreateMapForCar()
    {
        CreateMap<Car, CarModel>().ReverseMap();
        CreateMap<CarCreateModel, Car>();
    }

    private void CreateMapForCity()
    {
        CreateMap<City, CityModel>().ReverseMap();
        CreateMap<CityCreateModel, City>();
    }

    private void CreateMapForSalesman()
    {
        CreateMap<Salesman, SalesmanModel>().ReverseMap();
        CreateMap<SalesmanCreateModel, Salesman>();
    }

    private void CreateMapForAcceptanceAct()
    {
        CreateMap<AcceptanceAct, AcceptanceActModel>()
            .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<AcceptanceAct, AcceptanceActListItemModel>()
            .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.BuyerFullName,
                opt => opt.MapFrom(src =>
                    $"Компания: {src.Buyer.Name}, имя: {src.Buyer.FirstName}, фамилия: {src.Buyer.Surname}"))
            .ForMember(dest => dest.SalesmanFullName,
                opt => opt.MapFrom(src =>
                    $"Компания: {src.Salesman.Name}, имя: {src.Salesman.FirstName}, фамилия: {src.Salesman.Surname}"))
            .ForMember(dest => dest.CarMark, opt => opt.MapFrom(src => src.Car.Mark))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
            .ForMember(dest => dest.CarLicensePlate, opt => opt.MapFrom(src => src.Car.LicensePlate));

        CreateMap<AcceptanceActCreateModel, AcceptanceAct>();

        CreateMap<AcceptanceActUpdateModel, AcceptanceAct>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}