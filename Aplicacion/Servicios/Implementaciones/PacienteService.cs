using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Pacientes;
using FluentValidation;

namespace Aplicacion.Servicios.Implementaciones;

public class PacienteService : IPacienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CrearPacienteDTO> _crearValidacion;
    private readonly IValidator<ActualizarPacienteDTO> _actualizarValidator;

    public PacienteService(IUnitOfWork unitOfWork, IMapper mapper,
        IValidator<CrearPacienteDTO> crearValidacion,
        IValidator<ActualizarPacienteDTO> actualizarValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _crearValidacion = crearValidacion;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<ResultadoAccion<PacienteDTO>> ObtenerPorIdAsync(int id)
    {
        var paciente = await _unitOfWork.Pacientes.ObtenerPorIdAsync(id);
        if (paciente == null)
            return ResultadoAccion<PacienteDTO>.Falla("Paciente no encontrado");
        var dto = _mapper.Map<PacienteDTO>(paciente);
        return ResultadoAccion<PacienteDTO>.Exito(dto);
    }

    public async Task<ResultadoAccion<IEnumerable<PacienteDTO>>> ObtenerTodosAsync()
    {
        var pacientes = await _unitOfWork.Pacientes.ObtenerTodosAsync();
        var dtos = _mapper.Map<IEnumerable<PacienteDTO>>(pacientes);
        return ResultadoAccion<IEnumerable<PacienteDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<IEnumerable<PacienteDTO>>> ObtenerActivosSinHistoriaAsync()
    {
        var pacientes = await _unitOfWork.Pacientes.ObtenerActivosSinHistoriaAsync();
        var dtos = _mapper.Map<IEnumerable<PacienteDTO>>(pacientes);
        return ResultadoAccion<IEnumerable<PacienteDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<PacienteDTO>> CrearAsync(CrearPacienteDTO dto)
    {
// Validar
        var validacion = await _crearValidacion.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<PacienteDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

// Verificar duplicado por identificación
        if (await ExistePorIdentificacionAsync(dto.NumeroDocumento))
            return ResultadoAccion<PacienteDTO>.Falla("Ya existe un paciente con ese número de identificación");
// Mapear y crear paciente
        var paciente = _mapper.Map<Paciente>(dto);
        await _unitOfWork.Pacientes.AgregarAsync(paciente);
        await _unitOfWork.GuardarCambiosAsync();
        var pacienteCreado = await _unitOfWork.Pacientes.ObtenerPorIdAsync(paciente.Id);
        var dtoResultado = _mapper.Map<PacienteDTO>(pacienteCreado);
        return ResultadoAccion<PacienteDTO>.Exito(dtoResultado, "Paciente creado exitosamente");
    }

    public async Task<ResultadoAccion<PacienteDTO>> ActualizarAsync(ActualizarPacienteDTO dto)
    {
        var validacion = await _actualizarValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ResultadoAccion<PacienteDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        var paciente = await _unitOfWork.Pacientes.ObtenerPorIdAsync(dto.Id);
        if (paciente == null)
            return ResultadoAccion<PacienteDTO>.Falla("Paciente no encontrado");
        _mapper.Map(dto, paciente);
        paciente.FechaDeModificacion = DateTime.UtcNow;
        _unitOfWork.Pacientes.Actualizar(paciente);
        await _unitOfWork.GuardarCambiosAsync();
        var dtoResultado = _mapper.Map<PacienteDTO>(paciente);
        return ResultadoAccion<PacienteDTO>.Exito(dtoResultado, "Paciente actualizado");
    }

    public async Task<ResultadoAccion> EliminarAsync(int id)
    {
        var paciente = await _unitOfWork.Pacientes.ObtenerPorIdAsync(id);
        if (paciente == null)
            return ResultadoAccion.Falla("Paciente no encontrado");

// Borrado lógico
        paciente.Eliminado = true;
        paciente.FechaDeEliminacion = DateTime.UtcNow;
        paciente.Activo = false;
        _unitOfWork.Pacientes.Actualizar(paciente);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion.Exito("Paciente eliminado (borrado lógico)");
    }

    public async Task<bool> ExistePorIdentificacionAsync(string identificacion)
    {
        var pacientes = await _unitOfWork.Pacientes.BuscarAsync(p => p.NumeroDocumento == identificacion);
        return pacientes.Any();
    }
}