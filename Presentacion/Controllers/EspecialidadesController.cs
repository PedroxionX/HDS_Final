using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/especialidades")]
public class EspecialidadesController : ApiControllerBase
{
    private readonly IEspecialidadService _especialidadService;

    public EspecialidadesController(IEspecialidadService especialidadService)
    {
        _especialidadService = especialidadService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _especialidadService.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _especialidadService.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearEspecialidadDTO dto)
    {
        var resultado = await _especialidadService.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEspecialidadDTO dto)
    {
        if (dto.Id != 0 && dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        dto.Id = id;
        var resultado = await _especialidadService.ActualizarAsync(id, dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _especialidadService.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

