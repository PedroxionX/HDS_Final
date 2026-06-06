using Aplicacion.Abstracciones;
using Aplicacion.DTOs.Doctores;
using Aplicacion.Helpers;
using Aplicacion.Servicios.Interfaces;
using AutoMapper;
using Dominio.Entidades.Doctores;
namespace Aplicacion.Servicios.Implementaciones;
public class DoctorServicio : IDoctorServicio
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DoctorServicio(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ResultadoAccion<List<DoctorDTO>>> ObtenerTodosAsync()
    {
        var doctores = await _unitOfWork.Doctores.ObtenerTodosAsync();
        return ResultadoAccion<List<DoctorDTO>>.Exito(_mapper.Map<List<DoctorDTO>>(doctores));
    }
    public async Task<ResultadoAccion<DoctorDTO>> ObtenerPorIdAsync(int id)
    {
        var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
        if (doctor == null) return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");
        return ResultadoAccion<DoctorDTO>.Exito(_mapper.Map<DoctorDTO>(doctor));
    }
    public async Task<ResultadoAccion<DoctorDTO>> CrearAsync(CrearDoctorDTO dto)
    {
        var docs = await _unitOfWork.Doctores.BuscarAsync(d => d.Email == dto.Email);
        var existeEmail = docs.Any();
        if (existeEmail)
            return ResultadoAccion<DoctorDTO>.Falla("Ya existe un doctor con ese email");
        var doctor = _mapper.Map<Doctor>(dto);
        await _unitOfWork.Doctores.AgregarAsync(doctor);
        await _unitOfWork.GuardarCambiosAsync();
        return await ObtenerPorIdAsync(doctor.Id);
    }
    public async Task<ResultadoAccion<DoctorDTO>> ActualizarAsync(ActualizarDoctorDTO dto)
    {
        var doctorExistente = await _unitOfWork.Doctores.ObtenerPorIdAsync(dto.Id);
        if (doctorExistente == null) return ResultadoAccion<DoctorDTO>.Falla("Doctor no encontrado");
        var docs = await _unitOfWork.Doctores.BuscarAsync(d => d.Email == dto.Email && d.Id != dto.Id);
        var existeEmail = docs.Any();
        if (existeEmail)
            return ResultadoAccion<DoctorDTO>.Falla("Ya existe otro doctor con ese email");
        _mapper.Map(dto, doctorExistente);
        _unitOfWork.Doctores.Actualizar(doctorExistente);
        await _unitOfWork.GuardarCambiosAsync();
        return await ObtenerPorIdAsync(doctorExistente.Id);
    }
    public async Task<ResultadoAccion<bool>> EliminarAsync(int id)
    {
        var doctor = await _unitOfWork.Doctores.ObtenerPorIdAsync(id);
        if (doctor == null) return ResultadoAccion<bool>.Falla("Doctor no encontrado");
        var docs = await _unitOfWork.Doctores.BuscarAsync(d => d.Id == id && d.Citas.Any());
        var tieneCitas = docs.Any();
        if (tieneCitas)
            return ResultadoAccion<bool>.Falla("No se puede eliminar el doctor porque tiene citas/pacientes asociados");
        _unitOfWork.Doctores.Eliminar(doctor);
        await _unitOfWork.GuardarCambiosAsync();
        return ResultadoAccion<bool>.Exito(true);
    }
}
