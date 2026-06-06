using Aplicacion.Abstracciones;
using Dominio.Entidades.Doctores;
using Infraestructura.Data;
namespace Infraestructura.Repositorios;
public class DoctorRepositorio : RepositorioGenerico<Doctor>, IDoctorRepositorio
{
    public DoctorRepositorio(ContextoECE context) : base(context)
    {
    }
}
