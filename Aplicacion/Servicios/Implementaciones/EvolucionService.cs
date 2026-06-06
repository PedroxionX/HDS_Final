using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Evoluciones;
using FluentValidation;

namespace Aplicacion.Servicios.Implementaciones;

public class EvolucionService : IEvolucionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CrearEvolucionDTO> _crearValidator;
    private readonly IValidator<ActualizarEvolucionDTO> _actualizarValidator;

    public EvolucionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CrearEvolucionDTO> crearValidator,
        IValidator<ActualizarEvolucionDTO> actualizarValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<ResultadoAccion<EvolucionDTO>> ObtenerPorIdAsync(int id)
    {
        var evolucion = await _unitOfWork.Evoluciones.ObtenerConRelacionesAsync(id);
        if (evolucion == null)
            return ResultadoAccion<EvolucionDTO>.Falla("Evolucion no encontrada");
        return ResultadoAccion<EvolucionDTO>.Exito(_mapper.Map<EvolucionDTO>(evolucion));
    }

    public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerTodosAsync()
    {
        var evoluciones = await _unitOfWork.Evoluciones.ObtenerTodosConRelacionesAsync();
        var dtos = _mapper.Map<IEnumerable<EvolucionDTO>>(evoluciones);
        return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<IEnumerable<EvolucionDTO>>> ObtenerPorHistoriaClinicaAsync(int idHistoriaClinica)
    {
        var evoluciones = await _unitOfWork.Evoluciones.ObtenerPorHistoriaClinicaAsync(idHistoriaClinica);
        var dtos = _mapper.Map<IEnumerable<EvolucionDTO>>(evoluciones);
        return ResultadoAccion<IEnumerable<EvolucionDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<EvolucionDTO>> CrearAsync(CrearEvolucionDTO dto)
    {
        var validacion = await _crearValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<EvolucionDTO>.Falla("Datos invalidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var historia = await _unitOfWork.HistoriasClinicas.ObtenerPorIdAsync(dto.IdHistoriaClinica);
        if (historia == null)
            return ResultadoAccion<EvolucionDTO>.Falla("Historia clinica no encontrada o inactiva");

        var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.IdDoctor);
        if (doctor == null)
            return ResultadoAccion<EvolucionDTO>.Falla("Doctor no encontrado o inactivo");

        var evolucion = _mapper.Map<Evolucion>(dto);
        if (string.IsNullOrWhiteSpace(evolucion.Notas))
            evolucion.Notas = string.Empty;

        await _unitOfWork.Evoluciones.AgregarAsync(evolucion);
        await _unitOfWork.GuardarCambiosAsync();

        var creada = await _unitOfWork.Evoluciones.ObtenerConRelacionesAsync(evolucion.Id);
        var dtoResultado = creada != null ? _mapper.Map<EvolucionDTO>(creada) : _mapper.Map<EvolucionDTO>(evolucion);
        return ResultadoAccion<EvolucionDTO>.Exito(dtoResultado, "Evolucion creada exitosamente");
    }

    public async Task<ResultadoAccion<EvolucionDTO>> ActualizarAsync(ActualizarEvolucionDTO dto)
    {
        var validacion = await _actualizarValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<EvolucionDTO>.Falla("Datos invalidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(dto.Id);
        if (evolucion == null)
            return ResultadoAccion<EvolucionDTO>.Falla("Evolucion no encontrada");

        _mapper.Map(dto, evolucion);
        if (string.IsNullOrWhiteSpace(evolucion.Notas))
            evolucion.Notas = string.Empty;
        evolucion.FechaDeModificacion = DateTime.UtcNow;

        _unitOfWork.Evoluciones.Actualizar(evolucion);
        await _unitOfWork.GuardarCambiosAsync();

        var actualizada = await _unitOfWork.Evoluciones.ObtenerConRelacionesAsync(evolucion.Id);
        var dtoResultado = actualizada != null ? _mapper.Map<EvolucionDTO>(actualizada) : _mapper.Map<EvolucionDTO>(evolucion);
        return ResultadoAccion<EvolucionDTO>.Exito(dtoResultado, "Evolucion actualizada");
    }

    public async Task<ResultadoAccion> EliminarAsync(int id)
    {
        var evolucion = await _unitOfWork.Evoluciones.ObtenerPorIdAsync(id);
        if (evolucion == null)
            return ResultadoAccion.Falla("Evolucion no encontrada");

        evolucion.Eliminado = true;
        evolucion.Activo = false;
        evolucion.FechaDeEliminacion = DateTime.UtcNow;
        _unitOfWork.Evoluciones.Actualizar(evolucion);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion.Exito("Evolucion eliminada (borrado logico)");
    }
}

