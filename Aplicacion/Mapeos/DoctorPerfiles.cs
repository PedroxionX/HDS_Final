using Aplicacion.DTOs.Doctores;
using AutoMapper;
using Dominio.Entidades.Doctores;
namespace Aplicacion.Mapeos;
public class DoctorPerfiles : Profile
{
    public DoctorPerfiles()
    {
        CreateMap<Doctor, DoctorDTO>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.NombreCompleto))
            .ForMember(dest => dest.NombreEspecialidad, opt => opt.MapFrom(src => src.Especialidad.Nombre));
        CreateMap<CrearDoctorDTO, Doctor>()
            .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombre)) // Simplificado; ideal separar Nombres/Apellidos si es un solo string en UI
            .ForMember(dest => dest.Apellidos, opt => opt.Ignore());
        CreateMap<ActualizarDoctorDTO, Doctor>()
            .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Apellidos, opt => opt.Ignore());
    }
}
