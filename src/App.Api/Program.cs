using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Infra.Persistence.Ado.Repos;
using App.Application.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<IClinicService, ClinicService>();


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
