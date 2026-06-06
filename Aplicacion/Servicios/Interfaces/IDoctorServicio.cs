using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;
namespace Aplicacion.Servicios.Interfaces;
public interface IDoctorServicio
{
    Task<ResultadoAccion<List<DoctorDTO>>> ObtenerTodosAsync();
    Task<ResultadoAccion<DoctorDTO>> ObtenerPorIdAsync(int id);
    Task<ResultadoAccion<DoctorDTO>> CrearAsync(CrearDoctorDTO dto);
    Task<ResultadoAccion<DoctorDTO>> ActualizarAsync(ActualizarDoctorDTO dto);
    Task<ResultadoAccion<bool>> EliminarAsync(int id);
}
