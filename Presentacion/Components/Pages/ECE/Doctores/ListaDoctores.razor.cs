using Aplicacion.DTOs.Doctores;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
using CurrieTechnologies.Razor.SweetAlert2;
namespace Presentacion.Components.Pages.ECE.Doctores;
public partial class ListaDoctores
{
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    private List<DoctorDTO>? Doctores { get; set; }
    private bool Cargando { get; set; } = true;
    private string? MensajeError { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await CargarDoctores();
    }
    private async Task CargarDoctores()
    {
        Cargando = true;
        MensajeError = null;
        var resultado = await DoctorServicio.ObtenerTodosAsync();
        if (resultado.Exitoso)
        {
            Doctores = resultado.Datos;
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al cargar doctores: " + resultado.Mensaje);
        }
        Cargando = false;
    }
    private void IrACrearNuevo()
    {
        NavigationManager.NavigateTo("/crear-doctor");
    }
    private void VerDetalles(int id)
    {
        NavigationManager.NavigateTo($"/detalles-doctor/{id}");
    }
    private void EditarDoctor(int id)
    {
        NavigationManager.NavigateTo($"/editar-doctor/{id}");
    }
    private async Task ConfirmarEliminar(int id, string nombre)
    {
        string mensajeConfirmacion = $"Los datos del doctor: {nombre} se eliminarán de forma permanente del sistema.";
        var result = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = "¿Estás seguro de borrar?",
            Text = mensajeConfirmacion,
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ConfirmButtonText = "Sí, eliminar",
            CancelButtonText = "Cancelar"
        });
        var confirmado = !string.IsNullOrEmpty(result.Value);
        if (confirmado)
        {
            var resultado = await DoctorServicio.EliminarAsync(id);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito($"Doctor {nombre} eliminado correctamente.");
                await CargarDoctores();
                StateHasChanged();
            }
            else
            {
                await Swal.FireAsync("Error", "Error al eliminar: " + resultado.Mensaje, SweetAlertIcon.Error);
            }
        }
        else
        {
            await Toastr.MsgInformacion($"Acción cancelada: Los datos del doctor {nombre} fueron conservados.");
        }
    }
}
