using Dominio.Entidades.Base;
using Dominio.Entidades.Evoluciones;
using Dominio.Entidades.Pacientes;

namespace Dominio.Entidades.HistoriasClinicas;

public class HistoriaClinica : EntidadBase
{
    public int IdPaciente { get; set; }
    public DateTime FechaApertura { get; set; } = DateTime.UtcNow;
    public string Alergias { get; set; } = string.Empty;
    public string AntecedentesFamiliares { get; set; } = string.Empty;
    public string AntecedentesPersonales { get; set; } = string.Empty;
    public virtual Paciente Paciente { get; set; } = null!;
    public virtual ICollection<Evolucion> Evoluciones { get; set; } = new List<Evolucion>();
}