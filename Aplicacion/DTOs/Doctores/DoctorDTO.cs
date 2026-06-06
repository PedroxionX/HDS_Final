using System;
namespace Aplicacion.DTOs.Doctores;
public class DoctorDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdEspecialidad { get; set; }
    public string NombreEspecialidad { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaContratacion { get; set; }
    public bool Activo { get; set; }
}
