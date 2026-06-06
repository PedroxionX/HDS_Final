namespace Aplicacion.DTOs.Especialidades;
public class EspecialidadDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public int CantidadMedicos { get; set; }
}
