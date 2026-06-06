using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Especialidades;
public partial class EditarEspecialidad : ComponentBase
{
    [Parameter] public int id { get; set; }
    [Inject] private IEspecialidadService especialidadService { get; set; } = null!;
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    protected EspecialidadDTO? especialidadOriginal;
    protected ActualizarEspecialidadDTO especialidadEditar = new();
    protected override async Task OnInitializedAsync()
    {
        var resultado = await especialidadService.ObtenerPorIdAsync(id);
        if (resultado.Exitoso && resultado.Datos != null)
        {
            especialidadOriginal = resultado.Datos;
            especialidadEditar = new ActualizarEspecialidadDTO
            {
                Id = especialidadOriginal.Id,
                Nombre = especialidadOriginal.Nombre,
                Descripcion = especialidadOriginal.Descripcion,
                Activo = especialidadOriginal.Activo
            };
        }
        else
        {
            await Toastr.MsgError("No se encontró la especialidad.");
            Navigation.NavigateTo("/especialidades");
        }
    }
    protected async Task GrabarEspecialidad()
    {
        var resultado = await especialidadService.ActualizarAsync(id, especialidadEditar);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Especialidad actualizada con éxito");
            Navigation.NavigateTo("/especialidades");
        }
        else
        {
            await Toastr.MsgError(resultado.Mensaje ?? "Error al actualizar la especialidad");
        }
    }
    protected void Cancelar()
    {
        Navigation.NavigateTo("/especialidades");
    }
}
