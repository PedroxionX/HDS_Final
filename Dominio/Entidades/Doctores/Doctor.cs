using Dominio.Entidades.Base;
using Dominio.Entidades.Citas;
using Dominio.Entidades.Especialidades;

namespace Dominio.Entidades.Doctores;

public class Doctor : EntidadBase
{
    public int IdEspecialidad { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HorarioAtencion { get; set; } = string.Empty;
    public virtual Especialidad Especialidad { get; set; } = null!;
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public string NombreCompleto => $"{Nombres} {Apellidos}";
}