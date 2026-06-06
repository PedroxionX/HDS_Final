using Aplicacion.DTOs.Especialidades;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using Aplicacion.Abstracciones;
using Dominio.Entidades.Especialidades;
using AutoMapper;
namespace Aplicacion.Servicios.Implementaciones;
public class EspecialidadService : IEspecialidadService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EspecialidadService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ResultadoAccion<IEnumerable<EspecialidadDTO>>> ObtenerTodosAsync()
    {
        var especialidades = await _unitOfWork.Especialidades.ObtenerTodosAsync();
        var dtos = _mapper.Map<IEnumerable<EspecialidadDTO>>(especialidades);
        return ResultadoAccion<IEnumerable<EspecialidadDTO>>.Exito(dtos);
    }
    public async Task<ResultadoAccion<EspecialidadDTO>> ObtenerPorIdAsync(int id)
    {
        var especialidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
        if (especialidad == null) return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrada.");
        return ResultadoAccion<EspecialidadDTO>.Exito(_mapper.Map<EspecialidadDTO>(especialidad));
    }
    public async Task<ResultadoAccion<EspecialidadDTO>> CrearAsync(CrearEspecialidadDTO dto)
    {
        var busqueda = await _unitOfWork.Especialidades.BuscarAsync(e => e.Nombre == dto.Nombre);
        var existe = busqueda.Any();
        if (existe) return ResultadoAccion<EspecialidadDTO>.Falla("Ya existe una especialidad con ese nombre.");
        var entidad = _mapper.Map<Especialidad>(dto);
        entidad.Activo = true;
        await _unitOfWork.Especialidades.AgregarAsync(entidad);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion<EspecialidadDTO>.Exito(_mapper.Map<EspecialidadDTO>(entidad));
    }
    public async Task<ResultadoAccion<EspecialidadDTO>> ActualizarAsync(int id, ActualizarEspecialidadDTO dto)
    {
        var entidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
        if (entidad == null) return ResultadoAccion<EspecialidadDTO>.Falla("Especialidad no encontrada.");
        if (entidad.Nombre != dto.Nombre)
        {
            var busqueda = await _unitOfWork.Especialidades.BuscarAsync(e => e.Nombre == dto.Nombre && e.Id != id);
            var existe = busqueda.Any();
            if (existe) return ResultadoAccion<EspecialidadDTO>.Falla("Ya existe otra especialidad con ese nombre.");
        }
        _mapper.Map(dto, entidad);
        _unitOfWork.Especialidades.Actualizar(entidad);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion<EspecialidadDTO>.Exito(_mapper.Map<EspecialidadDTO>(entidad));
    }
    public async Task<ResultadoAccion<bool>> EliminarAsync(int id)
    {
        var entidad = await _unitOfWork.Especialidades.ObtenerPorIdAsync(id);
        if (entidad == null) return ResultadoAccion<bool>.Falla("Especialidad no encontrada.");
        if (entidad.Doctores != null && entidad.Doctores.Any()) return ResultadoAccion<bool>.Falla("No se puede eliminar la especialidad porque tiene médicos asociados.");
        _unitOfWork.Especialidades.Eliminar(entidad);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion<bool>.Exito(true);
    }
}
