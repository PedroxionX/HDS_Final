using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Helpers;

namespace Aplicacion.Servicios.Interfaces;

public interface IEvolucionService
{
    Task<ResultadoAccion<EvolucionDTO>> ObtenerPorIdAsync(int id);
    Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerTodosAsync();
    Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica);
    Task<ResultadoAccion<EvolucionDTO>> CrearAsync(CrearEvolucionDTO dto);
    Task<ResultadoAccion<EvolucionDTO>> ActualizarAsync(ActualizarEvolucionDTO dto);
    Task<ResultadoAccion> EliminarAsync(int id);
}

