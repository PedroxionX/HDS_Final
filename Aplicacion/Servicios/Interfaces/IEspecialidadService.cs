using Aplicacion.DTOs.Especialidades;
using Aplicacion.Helpers;
namespace Aplicacion.Servicios.Interfaces;
public interface IEspecialidadService
{
    Task<ResultadoAccion<IEnumerable<EspecialidadDTO>>> ObtenerTodosAsync();
    Task<ResultadoAccion<EspecialidadDTO>> ObtenerPorIdAsync(int id);
    Task<ResultadoAccion<EspecialidadDTO>> CrearAsync(CrearEspecialidadDTO dto);
    Task<ResultadoAccion<EspecialidadDTO>> ActualizarAsync(int id, ActualizarEspecialidadDTO dto);
    Task<ResultadoAccion<bool>> EliminarAsync(int id);
}
