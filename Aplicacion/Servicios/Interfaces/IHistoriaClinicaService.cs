using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces;

public interface IHistoriaClinicaService
{
    Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id);
    Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync();
    Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerActivasAsync();
    Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaDTO dto);
    Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaDTO dto);
    Task<ResultadoAccion> EliminarAsync(int id);
    Task<bool> ExisteHistoriaActivaAsync(int idPaciente, int? idHistoria = null);
}
