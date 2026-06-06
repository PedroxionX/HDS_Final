using AutoMapper;
using Aplicacion.DTOs.Especialidades;
using Dominio.Entidades.Especialidades;
namespace Aplicacion.Mapeos;
public class EspecialidadPerfiles : Profile
{
    public EspecialidadPerfiles()
    {
        CreateMap<Especialidad, EspecialidadDTO>()
            .ForMember(dest => dest.CantidadMedicos, opt => opt.MapFrom(src => src.Doctores.Count));
        CreateMap<CrearEspecialidadDTO, Especialidad>();
        CreateMap<ActualizarEspecialidadDTO, Especialidad>();
    }
}
