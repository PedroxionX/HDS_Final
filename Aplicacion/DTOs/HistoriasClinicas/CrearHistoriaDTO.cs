namespace Aplicacion.DTOs.HistoriasClinicas;

public class CrearHistoriaDTO
{
    public int IdPaciente { get; set; }
    public DateTime FechaApertura { get; set; } = DateTime.Today;
    public string? Alergias { get; set; }
    public string? AntecedentesFamiliares { get; set; }
    public string? AntecedentesPersonales { get; set; }
    public bool Activo { get; set; } = true;
}

