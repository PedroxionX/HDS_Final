using System;
namespace Aplicacion.DTOs.Doctores;
public class CrearDoctorDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdEspecialidad { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaContratacion { get; set; }
    public bool Activo { get; set; } = true;
}
