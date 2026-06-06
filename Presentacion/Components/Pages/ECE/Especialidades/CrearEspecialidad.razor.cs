using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Especialidades;
public partial class CrearEspecialidad : ComponentBase
{
    [Inject] private IEspecialidadService especialidadService { get; set; } = null!;
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    protected CrearEspecialidadDTO crearEspecialidadDTO = new();
    protected async Task GrabarEspecialidad()
    {
        var resultado = await especialidadService.CrearAsync(crearEspecialidadDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Especialidad creada con éxito");
            Navigation.NavigateTo("/especialidades");
        }
        else
        {
            await Toastr.MsgError(resultado.Mensaje ?? "Error al crear la especialidad");
        }
    }
    protected void Cancelar()
    {
        Navigation.NavigateTo("/especialidades");
    }
}
