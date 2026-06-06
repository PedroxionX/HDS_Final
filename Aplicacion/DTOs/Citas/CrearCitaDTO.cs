using Dominio.Enumeraciones;

namespace Aplicacion.DTOs.Citas;

public class CrearCitaDTO
{
    public int IdPaciente { get; set; }
    public int IdDoctor { get; set; }
    public DateTime FechaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
}

