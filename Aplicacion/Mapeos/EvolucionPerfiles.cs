using Aplicacion.DTOs.Evoluciones;
using AutoMapper;
using Dominio.Entidades.Evoluciones;

namespace Aplicacion.Mapeos;

public class EvolucionPerfiles : Profile
{
    public EvolucionPerfiles()
    {
        CreateMap<CrearEvolucionDTO, Evolucion>();
        CreateMap<ActualizarEvolucionDTO, Evolucion>();
        CreateMap<Evolucion, EvolucionDTO>()
            .ForMember(d => d.NombrePaciente,
                opt => opt.MapFrom(s => s.HistoriaClinica.Paciente.Nombres + " " + s.HistoriaClinica.Paciente.Apellidos))
            .ForMember(d => d.NombreDoctor, opt => opt.MapFrom(s => s.Doctor.NombreCompleto))
            .ForMember(d => d.NombreEspecialidad, opt => opt.MapFrom(s => s.Doctor.Especialidad.Nombre));
    }
}

