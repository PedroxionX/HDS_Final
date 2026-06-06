using Aplicacion.DTOs.Evoluciones;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/evoluciones")]
public class EvolucionesController : ApiControllerBase
{
    private readonly IEvolucionService _evolucionService;

    public EvolucionesController(IEvolucionService evolucionService)
    {
        _evolucionService = evolucionService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _evolucionService.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _evolucionService.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpGet("por-historia/{idHistoriaClinica:int}")]
    public async Task<IActionResult> ObtenerPorHistoriaClinica(int idHistoriaClinica)
    {
        var resultado = await _evolucionService.ObtenerPorHistoriaClinicaAsync(idHistoriaClinica);
        return ToActionResult(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearEvolucionDTO dto)
    {
        var resultado = await _evolucionService.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEvolucionDTO dto)
    {
        if (dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        var resultado = await _evolucionService.ActualizarAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _evolucionService.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

