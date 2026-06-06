namespace Aplicacion.DTOs.Evoluciones;

public class EvolucionDTO
{
    public int Id { get; set; }
    public int IdHistoriaClinica { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public int IdDoctor { get; set; }
    public string NombreDoctor { get; set; } = string.Empty;
    public string NombreEspecialidad { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public bool Activo { get; set; }
    public bool Eliminado { get; set; }
    public DateTime FechaDeCreacion { get; set; }
    public DateTime? FechaDeModificacion { get; set; }
    public DateTime? FechaDeEliminacion { get; set; }
}

