namespace Aplicacion.DTOs.Evoluciones;

public class CrearEvolucionDTO
{
    public int IdHistoriaClinica { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public bool Activo { get; set; } = true;
}

