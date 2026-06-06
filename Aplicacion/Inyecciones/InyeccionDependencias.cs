using System.Reflection;
using Aplicacion.Servicios.Implementaciones;
using Aplicacion.Servicios.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion.Inyecciones;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(InyeccionDependencias).Assembly);
        // FluentValidation: registra todos los validadores del ensamblado actual
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Servicios
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IEspecialidadService, EspecialidadService>();
        services.AddScoped<IDoctorServicio, DoctorServicio>();
        services.AddScoped<ICitaService, CitaService>();
        services.AddScoped<IHistoriaClinicaService, HistoriaClinicaService>();
        services.AddScoped<IEvolucionService, EvolucionService>();
        return services;
    }
}