using Aplicacion.DTOs.HistoriasClinicas;
using AutoMapper;
using Dominio.Entidades.HistoriasClinicas;

namespace Aplicacion.Mapeos;

public class HistoriaClinicaPerfiles : Profile
{
    public HistoriaClinicaPerfiles()
    {
        CreateMap<CrearHistoriaDTO, HistoriaClinica>();
        CreateMap<ActualizarHistoriaDTO, HistoriaClinica>();
        CreateMap<HistoriaClinica, HistoriaClinicaDTO>()
            .ForMember(d => d.NombrePaciente,
                opt => opt.MapFrom(s => s.Paciente.Nombres + " " + s.Paciente.Apellidos));
    }
}

