using Aplicacion.DTOs.Doctores;
using Aplicacion.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[Route("api/doctores")]
public class DoctoresController : ApiControllerBase
{
    private readonly IDoctorServicio _doctorServicio;

    public DoctoresController(IDoctorServicio doctorServicio)
    {
        _doctorServicio = doctorServicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _doctorServicio.ObtenerTodosAsync();
        return ToActionResult(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _doctorServicio.ObtenerPorIdAsync(id);
        return ToActionResult(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearDoctorDTO dto)
    {
        var resultado = await _doctorServicio.CrearAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarDoctorDTO dto)
    {
        if (dto.Id != id)
        {
            return BadRequest("El id de la ruta no coincide con el cuerpo.");
        }

        var resultado = await _doctorServicio.ActualizarAsync(dto);
        return ToActionResult(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _doctorServicio.EliminarAsync(id);
        return ToActionResult(resultado);
    }
}

