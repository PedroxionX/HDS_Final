using Aplicacion.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToActionResult(ResultadoAccion resultado)
    {
        if (resultado.Exitoso)
        {
            return Ok(resultado);
        }

        return EsNoEncontrado(resultado.Mensaje, resultado.Errores)
            ? NotFound(resultado)
            : BadRequest(resultado);
    }

    protected IActionResult ToActionResult<T>(ResultadoAccion<T> resultado)
    {
        if (resultado.Exitoso)
        {
            return Ok(resultado);
        }

        return EsNoEncontrado(resultado.Mensaje, resultado.Errores)
            ? NotFound(resultado)
            : BadRequest(resultado);
    }

    private static bool EsNoEncontrado(string mensaje, IEnumerable<string> errores)
    {
        if (mensaje.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
            mensaje.Contains("no existe", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return errores.Any(error =>
            error.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
            error.Contains("no existe", StringComparison.OrdinalIgnoreCase));
    }
}

