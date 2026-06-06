using Dominio.Enumeraciones;

namespace Aplicacion.DTOs.Citas;

public class CitaDTO
{
    public int Id { get; set; }
    public int IdPaciente { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public int IdDoctor { get; set; }
    public string NombreDoctor { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public EstadoCita Estado { get; set; }
    public bool Activo { get; set; }
    public bool Eliminado { get; set; }
    public DateTime FechaDeCreacion { get; set; }
    public DateTime? FechaDeModificacion { get; set; }
    public DateTime? FechaDeEliminacion { get; set; }
}

