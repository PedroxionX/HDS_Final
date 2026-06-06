namespace Aplicacion.DTOs.HistoriasClinicas;

public class ActualizarHistoriaDTO
{
    public int Id { get; set; }
    public int IdPaciente { get; set; }
    public DateTime FechaApertura { get; set; }
    public string? Alergias { get; set; }
    public string? AntecedentesFamiliares { get; set; }
    public string? AntecedentesPersonales { get; set; }
    public bool Activo { get; set; }
}

