using Aplicacion.Abstracciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.HistoriasClinicas;
using FluentValidation;

namespace Aplicacion.Servicios.Implementaciones;

public class HistoriaClinicaService : IHistoriaClinicaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CrearHistoriaDTO> _crearValidator;
    private readonly IValidator<ActualizarHistoriaDTO> _actualizarValidator;

    public HistoriaClinicaService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CrearHistoriaDTO> crearValidator,
        IValidator<ActualizarHistoriaDTO> actualizarValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<ResultadoAccion<HistoriaClinicaDTO>> ObtenerPorIdAsync(int id)
    {
        var historia = await _unitOfWork.HistoriasClinicas.ObtenerConPacienteAsync(id);
        if (historia == null)
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clínica no encontrada");
        return ResultadoAccion<HistoriaClinicaDTO>.Exito(_mapper.Map<HistoriaClinicaDTO>(historia));
    }

    public async Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerTodosAsync()
    {
        var historias = await _unitOfWork.HistoriasClinicas.ObtenerTodosConPacienteAsync();
        var dtos = _mapper.Map<IEnumerable<HistoriaClinicaDTO>>(historias);
        return ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>> ObtenerActivasAsync()
    {
        var historias = await _unitOfWork.HistoriasClinicas.ObtenerActivasConPacienteAsync();
        var dtos = _mapper.Map<IEnumerable<HistoriaClinicaDTO>>(historias);
        return ResultadoAccion<IEnumerable<HistoriaClinicaDTO>>.Exito(dtos);
    }

    public async Task<ResultadoAccion<HistoriaClinicaDTO>> CrearAsync(CrearHistoriaDTO dto)
    {
        var validacion = await _crearValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        if (dto.Activo && await ExisteHistoriaActivaAsync(dto.IdPaciente))
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("El paciente ya tiene una historia clínica activa.");

        var historia = _mapper.Map<HistoriaClinica>(dto);
        historia.Alergias = string.IsNullOrWhiteSpace(historia.Alergias) ? string.Empty : historia.Alergias;
        historia.AntecedentesFamiliares = string.IsNullOrWhiteSpace(historia.AntecedentesFamiliares) ? string.Empty : historia.AntecedentesFamiliares;
        historia.AntecedentesPersonales = string.IsNullOrWhiteSpace(historia.AntecedentesPersonales) ? string.Empty : historia.AntecedentesPersonales;

        await _unitOfWork.HistoriasClinicas.AgregarAsync(historia);
        await _unitOfWork.GuardarCambiosAsync();

        var creada = await _unitOfWork.HistoriasClinicas.ObtenerConPacienteAsync(historia.Id);
        var dtoResultado = creada != null ? _mapper.Map<HistoriaClinicaDTO>(creada) : _mapper.Map<HistoriaClinicaDTO>(historia);
        return ResultadoAccion<HistoriaClinicaDTO>.Exito(dtoResultado, "Historia clínica creada exitosamente");
    }

    public async Task<ResultadoAccion<HistoriaClinicaDTO>> ActualizarAsync(ActualizarHistoriaDTO dto)
    {
        var validacion = await _actualizarValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("Datos inválidos",
                validacion.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var historia = await _unitOfWork.HistoriasClinicas.ObtenerConPacienteAsync(dto.Id);
        if (historia == null)
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("Historia clínica no encontrada");

        if (dto.Activo && await ExisteHistoriaActivaAsync(dto.IdPaciente, dto.Id))
            return ResultadoAccion<HistoriaClinicaDTO>.Falla("El paciente ya tiene una historia clínica activa.");

        _mapper.Map(dto, historia);
        historia.Alergias = string.IsNullOrWhiteSpace(historia.Alergias) ? string.Empty : historia.Alergias;
        historia.AntecedentesFamiliares = string.IsNullOrWhiteSpace(historia.AntecedentesFamiliares) ? string.Empty : historia.AntecedentesFamiliares;
        historia.AntecedentesPersonales = string.IsNullOrWhiteSpace(historia.AntecedentesPersonales) ? string.Empty : historia.AntecedentesPersonales;
        historia.FechaDeModificacion = DateTime.UtcNow;

        _unitOfWork.HistoriasClinicas.Actualizar(historia);
        await _unitOfWork.GuardarCambiosAsync();

        var actualizada = await _unitOfWork.HistoriasClinicas.ObtenerConPacienteAsync(historia.Id);
        var dtoResultado = actualizada != null ? _mapper.Map<HistoriaClinicaDTO>(actualizada) : _mapper.Map<HistoriaClinicaDTO>(historia);
        return ResultadoAccion<HistoriaClinicaDTO>.Exito(dtoResultado, "Historia clínica actualizada");
    }

    public async Task<ResultadoAccion> EliminarAsync(int id)
    {
        var historia = await _unitOfWork.HistoriasClinicas.ObtenerConPacienteAsync(id);
        if (historia == null)
            return ResultadoAccion.Falla("Historia clínica no encontrada");

        historia.Eliminado = true;
        historia.Activo = false;
        historia.FechaDeEliminacion = DateTime.UtcNow;
        _unitOfWork.HistoriasClinicas.Actualizar(historia);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion.Exito("Historia clínica eliminada (borrado lógico)");
    }

    public async Task<bool> ExisteHistoriaActivaAsync(int idPaciente, int? idHistoria = null)
    {
        return await _unitOfWork.HistoriasClinicas.ExisteHistoriaActivaAsync(idPaciente, idHistoria);
    }
}
