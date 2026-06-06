using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/pacientes")]
public class PacientesController : ApiControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _pacienteService.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _pacienteService.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpGet("activos-sin-historia")]
    public async Task<IActionResult> ObtenerActivosSinHistoria()
    {
        var resultado = await _pacienteService.ObtenerActivosSinHistoriaAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("existe-identificacion")]
    public async Task<IActionResult> ExistePorIdentificacion([FromQuery] string identificacion)
    {
        var existe = await _pacienteService.ExistePorIdentificacionAsync(identificacion);
        return Ok(existe);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearPacienteDTO dto)
    {
        var resultado = await _pacienteService.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarPacienteDTO dto)
    {
        if (dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        var resultado = await _pacienteService.ActualizarAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _pacienteService.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

