using Aplicacion.DTOs.HistoriasClinicas;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas;

public partial class ListaHistorias
{
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;

    private List<HistoriaClinicaDTO>? Historias { get; set; }
    private bool Cargando { get; set; } = true;
    private string? MensajeError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await CargarHistorias();
    }

    private async Task CargarHistorias()
    {
        Cargando = true;
        MensajeError = null;
        var resultado = await HistoriaService.ObtenerTodosAsync();
        if (resultado.Exitoso)
        {
            Historias = resultado.Datos?.ToList() ?? new List<HistoriaClinicaDTO>();
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al cargar historias clínicas: " + resultado.Mensaje);
        }
        Cargando = false;
    }

    private void IrACrear() => NavigationManager.NavigateTo("/crear-historia");
    private void VerDetalles(int id) => NavigationManager.NavigateTo($"/detalles-historia/{id}");
    private void Editar(int id) => NavigationManager.NavigateTo($"/editar-historia/{id}");

    private async Task ConfirmarEliminar(int id, string nombrePaciente)
    {
        string mensajeConfirmacion = $"La historia clínica de {nombrePaciente} se eliminará de forma lógica.";
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
            var resultado = await HistoriaService.EliminarAsync(id);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Historia clínica eliminada correctamente.");
                await CargarHistorias();
                StateHasChanged();
            }
            else
            {
                await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
        }
        else
        {
            await Toastr.MsgInformacion("Acción cancelada: la historia clínica fue conservada.");
        }
    }

    private string ObtenerTextoCorto(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return "—";
        return texto.Length > 40 ? texto.Substring(0, 40) + "..." : texto;
    }
}

