using Aplicacion.DTOs.Evoluciones;
using Microsoft.AspNetCore.Components;

namespace Presentacion.Components.Pages.ECE.Evoluciones;

public partial class DetalleEvolucion
{
    [Parameter] public int Id { get; set; }

    private EvolucionDTO? Evolucion { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var resultado = await EvolucionService.ObtenerPorIdAsync(Id);
        if (resultado.Exitoso)
        {
            Evolucion = resultado.Datos;
        }
        else
        {
            MensajeError = "No se pudo cargar la informacion de la evolucion. " + resultado.Mensaje;
        }
        Cargando = false;
    }

    private void VolverAlListado()
    {
        NavigationManager.NavigateTo("/evoluciones");
    }

    private void EditarEvolucion(int evolucionId)
    {
        NavigationManager.NavigateTo($"/editar-evolucion/{evolucionId}");
    }

    private string MostrarTexto(string? texto)
    {
        return string.IsNullOrWhiteSpace(texto) ? "—" : texto;
    }
}

