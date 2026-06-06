using Dominio.Entidades.HistoriasClinicas;

namespace Aplicacion.Abstracciones;

public interface IHistoriaClinicaRepositorio : IRepositorioGenerico<HistoriaClinica>
{
    Task<HistoriaClinica?> ObtenerConPacienteAsync(int id);
    Task<IEnumerable<HistoriaClinica>> ObtenerTodosConPacienteAsync();
    Task<IEnumerable<HistoriaClinica>> ObtenerActivasConPacienteAsync();
    Task<bool> ExisteHistoriaActivaAsync(int idPaciente, int? idHistoria = null);
}
