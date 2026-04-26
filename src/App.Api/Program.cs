using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Infra.Persistence.Ado.Repos;
using App.Application.Services;
using Scalar.AspNetCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using App.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

//Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<IClinicService, ClinicService>();

// Register all validators from an assembly
builder.Services.AddValidatorsFromAssemblyContaining<GetClinicByNameRequestValidator>();
// integrate with automatic model validation 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
