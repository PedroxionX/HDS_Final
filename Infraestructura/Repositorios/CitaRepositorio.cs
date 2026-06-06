using Aplicacion.Abstracciones;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class CitaRepositorio : RepositorioGenerico<Cita>, ICitaRepositorio
{
    public CitaRepositorio(ContextoECE contexto) : base(contexto)
    {
    }

    public async Task<Cita?> ObtenerConRelacionesAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Paciente)
            .Include(c => c.Doctor)
            .FirstOrDefaultAsync(c => c.Id == id && c.Activo && !c.Eliminado);
    }

    public async Task<IEnumerable<Cita>> ObtenerTodosConRelacionesAsync()
    {
        return await _dbSet
            .Include(c => c.Paciente)
            .Include(c => c.Doctor)
            .Where(c => c.Activo && !c.Eliminado)
            .ToListAsync();
    }

    public async Task<bool> ExisteCitaEnHorarioAsync(int idDoctor, DateTime fechaHora, int? idCita = null)
    {
        var query = _dbSet.Where(c =>
            c.IdDoctor == idDoctor &&
            c.FechaHora == fechaHora &&
            c.Activo &&
            !c.Eliminado &&
            c.Estado != EstadoCita.Cancelada);

        if (idCita.HasValue)
            query = query.Where(c => c.Id != idCita.Value);

        return await query.AnyAsync();
    }
}

