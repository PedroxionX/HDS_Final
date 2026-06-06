using Microsoft.AspNetCore.Components;
using Aplicacion.DTOs.Especialidades;
using Aplicacion.Servicios.Interfaces;
using CurrieTechnologies.Razor.SweetAlert2;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Especialidades;
public partial class ListaEspecialidades : ComponentBase
{
    [Inject] private IEspecialidadService especialidadService { get; set; } = null!;
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    protected List<EspecialidadDTO>? especialidades;
    protected override async Task OnInitializedAsync()
    {
        await CargarEspecialidades();
    }
    protected async Task CargarEspecialidades()
    {
        var resultado = await especialidadService.ObtenerTodosAsync();
        if (resultado.Exitoso && resultado.Datos != null)
        {
            especialidades = resultado.Datos.ToList();
        }
        else
        {
            await Toastr.MsgError("Error al cargar especialidades: " + resultado.Mensaje);
            especialidades = new List<EspecialidadDTO>();
        }
    }
    protected void NavegarACrear() => Navigation.NavigateTo("/crear-especialidad");
    protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-especialidad/{id}");
    protected void Editar(int id) => Navigation.NavigateTo($"/editar-especialidad/{id}");
    protected async Task ConfirmarEliminar(int id, string nombre, int cantidadMedicos)
    {
        if (cantidadMedicos > 0)
        {
            await Swal.FireAsync("Atención", "No se puede eliminar la especialidad porque tiene médicos asociados. Debe reasignar o eliminar los médicos primero.", SweetAlertIcon.Warning);
            return;
        }
        var result = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = "¿Estás seguro de borrar?",
            Text = $"Los datos de la especialidad: {nombre} se eliminarán de forma permanente del sistema.",
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ConfirmButtonText = "Sí, eliminar",
            CancelButtonText = "Cancelar"
        });
        if (!string.IsNullOrEmpty(result.Value))
        {
            var resultado = await especialidadService.EliminarAsync(id);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito($"Especialidad {nombre} eliminada correctamente.");
                await CargarEspecialidades();
                StateHasChanged();
            }
            else
            {
                await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
        }
        else
        {
            await Toastr.MsgInformacion($"Acción cancelada: Los datos de la especialidad {nombre} fueron conservados.");
        }
    }
}
