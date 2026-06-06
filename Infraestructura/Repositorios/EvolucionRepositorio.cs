using Aplicacion.Abstracciones;
using Dominio.Entidades.Evoluciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class EvolucionRepositorio : RepositorioGenerico<Evolucion>, IEvolucionRepositorio
{
    public EvolucionRepositorio(ContextoECE contexto) : base(contexto)
    {
    }

    public async Task<Evolucion?> ObtenerConRelacionesAsync(int id)
    {
        return await _dbSet
            .Include(e => e.HistoriaClinica)
            .ThenInclude(h => h.Paciente)
            .Include(e => e.Doctor)
            .ThenInclude(d => d.Especialidad)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo && !e.Eliminado);
    }

    public async Task<IEnumerable<Evolucion>> ObtenerTodosConRelacionesAsync()
    {
        return await _dbSet
            .Include(e => e.HistoriaClinica)
            .ThenInclude(h => h.Paciente)
            .Include(e => e.Doctor)
            .ThenInclude(d => d.Especialidad)
            .Where(e => e.Activo && !e.Eliminado)
            .OrderByDescending(e => e.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Evolucion>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica)
    {
        return await _dbSet
            .Include(e => e.HistoriaClinica)
            .ThenInclude(h => h.Paciente)
            .Include(e => e.Doctor)
            .ThenInclude(d => d.Especialidad)
            .Where(e => e.IdHistoriaClinica == idHistoriaClinica && e.Activo && !e.Eliminado)
            .OrderByDescending(e => e.Fecha)
            .ToListAsync();
    }
}

