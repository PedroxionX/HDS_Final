namespace Infraestructura.Repositorios;

using Aplicacion.Abstracciones;
using Dominio.Entidades.Especialidades;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

public class EspecialidadRepositorio : RepositorioGenerico<Especialidad>, IEspecialidadRepositorio
{
    public EspecialidadRepositorio(ContextoECE contexto) : base(contexto)
    {
    }
}
