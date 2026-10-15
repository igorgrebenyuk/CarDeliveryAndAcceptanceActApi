using CarDelivery_AcceptanceActApi.Api.Automapper;
using CarDelivery_AcceptanceActApi.Implementations;
using CarDeliveryAndAcceptance.Common;
using CarDeliveryAndAcceptance.Context;
using CarDeliveryAndAcceptance.Dal.Contracts.Repositories;
using CarDeliveryAndAcceptance.Repositories;
using CarDeliveryAndAcceptance.Repositories.Contracts;
using CarDeliveryAndAcceptance.Services;
using CarDeliveryAndAcceptance.Services.Automapper;
using CarDeliveryAndAcceptance.Services.Contracts;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IDbWriterContext, DbWriterContext>();
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ApiProfile>();
    x.AddProfile<ServiceProfile>();
});

builder.Services.AddDbContext<CarDeliveryAndAcceptanceContext>(opts =>
    opts.UseNpgsql("Host=localhost;Port=5432;Database=CarDeliveryAndAcceptanceActApi;Username=postgres;Password="));
builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());
builder.Services.AddScoped<IReader>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());
builder.Services.AddScoped<IWriter>(x => x.GetRequiredService<CarDeliveryAndAcceptanceContext>());


builder.Services.AddControllers();
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