using CarDeliveryAndAcceptanceActApi.Api.Automapper;
using CarDeliveryAndAcceptanceActApi.Implementations;
using CarDeliveryAndAcceptanceActApi.Infrastructure;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Context;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Validators.Car;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Инфраструктура
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IIdentityProvider, IdentityProvider>();
builder.Services.AddScoped<IDbWriterContext, DbWriterContext>();

// Валидация
builder.Services.AddScoped<IValidateService, ValidateService>();
builder.Services.RegisterImplementationsOf<IValidator>(
    typeof(CarCreateModelValidator).Assembly,
    ServiceLifetime.Scoped);

// Репозитории
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<ISalesmanRepository, SalesmanRepository>();
builder.Services.AddScoped<IAcceptanceActRepository, AcceptanceActRepository>();

// Сервисы
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<ISalesmanService, SalesmanService>();
builder.Services.AddScoped<IAcceptanceActService, AcceptanceActService>();

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ApiProfile>();
    x.AddProfile<ServiceProfile>();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CarDeliveryAndAcceptanceContext>(opts =>
    opts.UseNpgsql(connectionString));
builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());
builder.Services.AddScoped<IReader>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());
builder.Services.AddScoped<IWriter>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());

// ExceptionFilter 
var controllers = builder.Services.AddControllers(opts => opts.Filters.Add<ExceptionFilter>());

if (builder.Environment.IsEnvironment("integration"))
{
    controllers.AddControllersAsServices();
}

builder.Services.AddHealthChecks();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("swagger/v1/swagger.json");
    app.UseSwaggerUI();
}

app.MapHealthChecks("health");
app.MapControllers();

app.Run();

/// <summary>
/// Маркерный класс для интеграционных тестов
/// </summary>
public partial class Program
{
}