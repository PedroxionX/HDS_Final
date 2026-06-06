using Aplicacion.DTOs.Evoluciones;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones;

public partial class ListaEvoluciones
{
    [Parameter] public int? IdHistoria { get; set; }

    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;

    private List<EvolucionDTO>? Evoluciones { get; set; }
    private List<ResumenPaciente> ResumenPacientes { get; set; } = new();
    private bool Cargando { get; set; } = true;
    private string? MensajeError { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await CargarEvoluciones();
    }

    private async Task CargarEvoluciones()
    {
        Cargando = true;
        MensajeError = null;
        var resultado = IdHistoria.HasValue
            ? await EvolucionService.ObtenerPorHistoriaClinicaAsync(IdHistoria.Value)
            : await EvolucionService.ObtenerTodosAsync();

        if (resultado.Exitoso)
        {
            Evoluciones = resultado.Datos?.ToList() ?? new List<EvolucionDTO>();
            ResumenPacientes = Evoluciones
                .GroupBy(e => e.NombrePaciente)
                .Select(g => new ResumenPaciente(g.Key, g.Count()))
                .OrderByDescending(g => g.Cantidad)
                .ToList();
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al cargar evoluciones: " + resultado.Mensaje);
        }

        Cargando = false;
    }

    private void IrACrear()
    {
        if (IdHistoria.HasValue)
            NavigationManager.NavigateTo($"/crear-evolucion/{IdHistoria.Value}");
        else
            NavigationManager.NavigateTo("/crear-evolucion");
    }

    private void VerDetalles(int id) => NavigationManager.NavigateTo($"/detalles-evolucion/{id}");
    private void Editar(int id) => NavigationManager.NavigateTo($"/editar-evolucion/{id}");

    private async Task ConfirmarEliminar(int id, string nombrePaciente)
    {
        string mensajeConfirmacion = $"La evolucion del paciente {nombrePaciente} se eliminara de forma logica.";
        var result = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = "¿Estas seguro de borrar?",
            Text = mensajeConfirmacion,
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ConfirmButtonText = "Si, eliminar",
            CancelButtonText = "Cancelar"
        });

        var confirmado = !string.IsNullOrEmpty(result.Value);
        if (confirmado)
        {
            var resultado = await EvolucionService.EliminarAsync(id);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Evolucion eliminada correctamente.");
                await CargarEvoluciones();
                StateHasChanged();
            }
            else
            {
                await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
        }
        else
        {
            await Toastr.MsgInformacion("Accion cancelada: la evolucion fue conservada.");
        }
    }

    private string ObtenerTextoCorto(string? texto, int max)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return "—";
        return texto.Length > max ? texto.Substring(0, max) + "..." : texto;
    }

    private sealed record ResumenPaciente(string NombrePaciente, int Cantidad);
}

