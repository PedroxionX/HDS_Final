using Aplicacion.DTOs.HistoriasClinicas;
using Microsoft.AspNetCore.Components;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas;

public partial class DetalleHistoria
{
    [Parameter] public int Id { get; set; }

    private HistoriaClinicaDTO? Historia { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var resultado = await HistoriaService.ObtenerPorIdAsync(Id);
        if (resultado.Exitoso)
        {
            Historia = resultado.Datos;
        }
        else
        {
            MensajeError = "No se pudo cargar la información de la historia clínica. " + resultado.Mensaje;
        }
        Cargando = false;
    }

    private void VolverAlListado()
    {
        NavigationManager.NavigateTo("/historias-clinicas");
    }

    private void EditarHistoria(int historiaId)
    {
        NavigationManager.NavigateTo($"/editar-historia/{historiaId}");
    }

    private void VerEvoluciones(int historiaId)
    {
        NavigationManager.NavigateTo($"/evoluciones/{historiaId}");
    }

    private string MostrarTexto(string? texto)
    {
        return string.IsNullOrWhiteSpace(texto) ? "—" : texto;
    }
}

