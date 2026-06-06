using Aplicacion.Abstracciones;
using Infraestructura.Data;

namespace Infraestructura.Repositorios;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextoECE _contexto;
    private IPacienteRepositorio? _pacienteRepositorio;
    private IEspecialidadRepositorio? _especialidadRepositorio;
    private IDoctorRepositorio? _doctorRepositorio;
    private ICitaRepositorio? _citaRepositorio;
    private IHistoriaClinicaRepositorio? _historiaClinicaRepositorio;
    private IEvolucionRepositorio? _evolucionRepositorio;

    public UnitOfWork(ContextoECE contexto)
    {
        _contexto = contexto;
    }

    public IPacienteRepositorio Pacientes => _pacienteRepositorio ??= new PacienteRepositorio(_contexto);
    public IEspecialidadRepositorio Especialidades => _especialidadRepositorio ??= new EspecialidadRepositorio(_contexto);
    public IDoctorRepositorio Doctores => _doctorRepositorio ??= new DoctorRepositorio(_contexto);
    public ICitaRepositorio Citas => _citaRepositorio ??= new CitaRepositorio(_contexto);
    public IHistoriaClinicaRepositorio HistoriasClinicas => _historiaClinicaRepositorio ??= new HistoriaClinicaRepositorio(_contexto);
    public IEvolucionRepositorio Evoluciones => _evolucionRepositorio ??= new EvolucionRepositorio(_contexto);

    public async Task<int> GuardarCambiosAsync()
    {
        return await _contexto.SaveChangesAsync();
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }
}