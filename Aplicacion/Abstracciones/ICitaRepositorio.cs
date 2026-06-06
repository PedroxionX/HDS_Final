using Dominio.Entidades.Citas;

namespace Aplicacion.Abstracciones;

public interface ICitaRepositorio : IRepositorioGenerico<Cita>
{
    Task<Cita?> ObtenerConRelacionesAsync(int id);
    Task<IEnumerable<Cita>> ObtenerTodosConRelacionesAsync();
    Task<bool> ExisteCitaEnHorarioAsync(int idDoctor, DateTime fechaHora, int? idCita = null);
}

