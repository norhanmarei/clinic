using App.Application.Interfaces.Repos;
using App.Application.Interfaces.Services;
using App.Infra.Persistence.Ado.Repos;
using App.Application.Services;
using Scalar.AspNetCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using App.Api.Middleware;
using Serilog;
using App.Application.DTOs;
using App.Application.Interfaces.Security;
using App.Infra.Security;

var builder = WebApplication.CreateBuilder(args);
//Serilog setup
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));

//Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<IClinicService, ClinicService>();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
// Register all validators from an assembly
builder.Services.AddValidatorsFromAssemblyContaining<GetClinicResponse>();
// integrate with automatic model validation 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
// register exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// register cors 
builder.Services.AddCors(options => 
    options.AddPolicy("CorsPolicy", policy => 
      {
        policy
        // .WithOrigins("https://localhost:7142", "http://localhost:5119")
        .AllowAnyHeader()
        .AllowAnyMethod();
      })
    );

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

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
// app.UseAuthentication();
// app.UseAuthorization();
// serilog automatic request logging
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
