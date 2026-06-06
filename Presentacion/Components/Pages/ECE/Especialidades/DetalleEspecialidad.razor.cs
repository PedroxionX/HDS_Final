using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Especialidades;
public partial class DetalleEspecialidad : ComponentBase
{
    [Parameter] public int id { get; set; }
    [Inject] private IEspecialidadService especialidadService { get; set; } = null!;
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    protected EspecialidadDTO? especialidad;
    protected override async Task OnInitializedAsync()
    {
        await CargarEspecialidad();
    }
    protected async Task CargarEspecialidad()
    {
        var resultado = await especialidadService.ObtenerPorIdAsync(id);
        if (resultado.Exitoso && resultado.Datos != null)
        {
            especialidad = resultado.Datos;
        }
        else
        {
            await Toastr.MsgError("No se encontró la especialidad.");
            Navigation.NavigateTo("/especialidades");
        }
    }
    protected void Volver() => Navigation.NavigateTo("/especialidades");
    protected void Editar() => Navigation.NavigateTo($"/editar-especialidad/{id}");
}
