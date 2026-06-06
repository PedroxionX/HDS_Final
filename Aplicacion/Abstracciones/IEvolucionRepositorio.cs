using Dominio.Entidades.Evoluciones;

namespace Aplicacion.Abstracciones;

public interface IEvolucionRepositorio : IRepositorioGenerico<Evolucion>
{
    Task<Evolucion?> ObtenerConRelacionesAsync(int id);
    Task<IEnumerable<Evolucion>> ObtenerTodosConRelacionesAsync();
    Task<IEnumerable<Evolucion>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica);
}

