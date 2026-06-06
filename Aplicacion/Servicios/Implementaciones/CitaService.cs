using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Citas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Citas;
using Dominio.Enumeraciones;
using FluentValidation;

namespace Aplicacion.Servicios.Implementaciones;

public class CitaService : ICitaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CrearCitaDTO> _crearValidator;
    private readonly IValidator<ActualizarCitaDTO> _actualizarValidator;

    public CitaService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CrearCitaDTO> crearValidator,
        IValidator<ActualizarCitaDTO> actualizarValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<ResultadoAccion<CitaDTO>> ObtenerPorIdAsync(int id)
    {
        var cita = await _unitOfWork.Citas.ObtenerConRelacionesAsync(id);
        if (cita == null)
            return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada");
        return ResultadoAccion<CitaDTO>.Exito(_mapper.Map<CitaDTO>(cita));
    }

    public async Task<ResultadoAccion<IEnumerable<CitaDTO>>> ObtenerTodosAsync()
    {
        var citas = await _unitOfWork.Citas.ObtenerTodosConRelacionesAsync();
        var dtos = _mapper.Map<IEnumerable<CitaDTO>>(citas);
        return ResultadoAccion<IEnumerable<CitaDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<CitaDTO>> CrearAsync(CrearCitaDTO dto)
    {
        var validacion = await _crearValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<CitaDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var disponible = await EstaDisponibleAsync(dto.IdDoctor, dto.FechaHora);
        if (!disponible)
            return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita en ese horario.");

        var cita = _mapper.Map<Cita>(dto);
        if (string.IsNullOrWhiteSpace(cita.Notas))
            cita.Notas = string.Empty;
        if (dto.Estado == EstadoCita.Cancelada)
            cita.FechaDeEliminacion = DateTime.UtcNow;

        await _unitOfWork.Citas.AgregarAsync(cita);
        await _unitOfWork.GuardarCambiosAsync();

        var creada = await _unitOfWork.Citas.ObtenerConRelacionesAsync(cita.Id);
        var dtoResultado = creada != null ? _mapper.Map<CitaDTO>(creada) : _mapper.Map<CitaDTO>(cita);
        return ResultadoAccion<CitaDTO>.Exito(dtoResultado, "Cita creada exitosamente");
    }

    public async Task<ResultadoAccion<CitaDTO>> ActualizarAsync(ActualizarCitaDTO dto)
    {
        var validacion = await _actualizarValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<CitaDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(dto.Id);
        if (cita == null)
            return ResultadoAccion<CitaDTO>.Falla("Cita no encontrada");

        var disponible = await EstaDisponibleAsync(dto.IdDoctor, dto.FechaHora, dto.Id);
        if (!disponible)
            return ResultadoAccion<CitaDTO>.Falla("El doctor ya tiene una cita en ese horario.");

        var estadoAnterior = cita.Estado;
        var notasActuales = cita.Notas;
        _mapper.Map(dto, cita);
        if (dto.Notas == null)
            cita.Notas = notasActuales;
        else if (string.IsNullOrWhiteSpace(cita.Notas))
            cita.Notas = string.Empty;
        cita.FechaDeModificacion = DateTime.UtcNow;

        if (estadoAnterior != EstadoCita.Cancelada && dto.Estado == EstadoCita.Cancelada)
            cita.FechaDeEliminacion = DateTime.UtcNow;

        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambiosAsync();

        var actualizada = await _unitOfWork.Citas.ObtenerConRelacionesAsync(cita.Id);
        var dtoResultado = actualizada != null ? _mapper.Map<CitaDTO>(actualizada) : _mapper.Map<CitaDTO>(cita);
        return ResultadoAccion<CitaDTO>.Exito(dtoResultado, "Cita actualizada");
    }

    public async Task<ResultadoAccion> EliminarAsync(int id)
    {
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(id);
        if (cita == null)
            return ResultadoAccion.Falla("Cita no encontrada");

        cita.Eliminado = true;
        cita.Activo = false;
        cita.FechaDeEliminacion = DateTime.UtcNow;
        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion.Exito("Cita eliminada (borrado lógico)");
    }

    public async Task<bool> EstaDisponibleAsync(int idDoctor, DateTime fechaHora, int? idCita = null)
    {
        var existe = await _unitOfWork.Citas.ExisteCitaEnHorarioAsync(idDoctor, fechaHora, idCita);
        return !existe;
    }
}
