using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

using API_Proj.Infastructure;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Features;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Development")));


// Add services to the container.

//builder.Services.AddControllers(options =>
//{
//    options.ReturnHttpNotAcceptable = true;
//}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();


var mapperConfig = new MapperConfiguration(mc => {
            mc.AddProfile(new Map());
            });

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
