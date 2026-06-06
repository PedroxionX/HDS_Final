using Aplicacion.DTOs.Doctores;
using Microsoft.AspNetCore.Components;
namespace Presentacion.Components.Pages.ECE.Doctores;
public partial class DetalleDoctor
{
    [Parameter]
    public int id { get; set; }
    private DoctorDTO? Doctor { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;
    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }
    private async Task CargarDatos()
    {
        Cargando = true;
        var resultado = await DoctorServicio.ObtenerPorIdAsync(id);
        if (resultado.Exitoso)
        {
            Doctor = resultado.Datos;
        }
        else
        {
            MensajeError = "No se pudo cargar la información del doctor. " + resultado.Mensaje;
        }
        Cargando = false;
    }
    private void VolverAlListado()
    {
        NavigationManager.NavigateTo("/doctores");
    }
    private void EditarDoctor(int doctorId)
    {
        NavigationManager.NavigateTo($"/editar-doctor/{doctorId}");
    }
}
