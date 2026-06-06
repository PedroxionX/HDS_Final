using Aplicacion.DTOs.Citas;
using AutoMapper;
using Dominio.Entidades.Citas;

namespace Aplicacion.Mapeos;

public class CitaPerfiles : Profile
{
    public CitaPerfiles()
    {
        CreateMap<CrearCitaDTO, Cita>();
        CreateMap<ActualizarCitaDTO, Cita>();
        CreateMap<Cita, CitaDTO>()
            .ForMember(d => d.NombrePaciente,
                opt => opt.MapFrom(s => s.Paciente.Nombres + " " + s.Paciente.Apellidos))
            .ForMember(d => d.NombreDoctor, opt => opt.MapFrom(s => s.Doctor.NombreCompleto));
    }
}
