using Aplicacion.Abstracciones;
using Dominio.Entidades.HistoriasClinicas;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class HistoriaClinicaRepositorio : RepositorioGenerico<HistoriaClinica>, IHistoriaClinicaRepositorio
{
    public HistoriaClinicaRepositorio(ContextoECE contexto) : base(contexto)
    {
    }

    public async Task<HistoriaClinica?> ObtenerConPacienteAsync(int id)
    {
        return await _dbSet
            .Include(h => h.Paciente)
            .FirstOrDefaultAsync(h => h.Id == id && !h.Eliminado);
    }

    public async Task<IEnumerable<HistoriaClinica>> ObtenerTodosConPacienteAsync()
    {
        return await _dbSet
            .Include(h => h.Paciente)
            .Where(h => !h.Eliminado)
            .ToListAsync();
    }

    public async Task<IEnumerable<HistoriaClinica>> ObtenerActivasConPacienteAsync()
    {
        return await _dbSet
            .Include(h => h.Paciente)
            .Where(h => h.Activo && !h.Eliminado)
            .ToListAsync();
    }

    public async Task<bool> ExisteHistoriaActivaAsync(int idPaciente, int? idHistoria = null)
    {
        var query = _dbSet.Where(h => h.IdPaciente == idPaciente && h.Activo && !h.Eliminado);
        if (idHistoria.HasValue)
            query = query.Where(h => h.Id != idHistoria.Value);
        return await query.AnyAsync();
    }
}
