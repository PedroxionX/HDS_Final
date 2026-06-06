using Aplicacion.DTOs.Citas;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/citas")]
public class CitasController : ApiControllerBase
{
    private readonly ICitaService _citaService;

    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _citaService.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _citaService.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpGet("disponible")]
    public async Task<IActionResult> EstaDisponible([FromQuery] int idDoctor, [FromQuery] DateTime fechaHora, [FromQuery] int? idCita)
    {
        var disponible = await _citaService.EstaDisponibleAsync(idDoctor, fechaHora, idCita);
        return Ok(disponible);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearCitaDTO dto)
    {
        var resultado = await _citaService.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCitaDTO dto)
    {
        if (dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        var resultado = await _citaService.ActualizarAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _citaService.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

