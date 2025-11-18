using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using PrazskaLitacka.Infrastructure.Services.Impl;
using PrazskaLitacka.Webapi.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGetPidDataService, GetPidDataService>();
builder.Services.AddTransient<IRaceEvaluationService, RaceEvaluationService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddHttpClient("XmlDataClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["XmlDataClient:BaseUrl"]!);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
