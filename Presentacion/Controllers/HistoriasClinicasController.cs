using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/historias-clinicas")]
public class HistoriasClinicasController : ApiControllerBase
{
    private readonly IHistoriaClinicaService _historiaClinicaService;

    public HistoriasClinicasController(IHistoriaClinicaService historiaClinicaService)
    {
        _historiaClinicaService = historiaClinicaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _historiaClinicaService.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _historiaClinicaService.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas()
    {
        var resultado = await _historiaClinicaService.ObtenerActivasAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("existe-activa")]
    public async Task<IActionResult> ExisteHistoriaActiva([FromQuery] int idPaciente, [FromQuery] int? idHistoria)
    {
        var existe = await _historiaClinicaService.ExisteHistoriaActivaAsync(idPaciente, idHistoria);
        return Ok(existe);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearHistoriaDTO dto)
    {
        var resultado = await _historiaClinicaService.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarHistoriaDTO dto)
    {
        if (dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        var resultado = await _historiaClinicaService.ActualizarAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _historiaClinicaService.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

