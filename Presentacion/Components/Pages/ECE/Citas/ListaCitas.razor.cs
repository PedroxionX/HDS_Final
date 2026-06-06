using Aplicacion.DTOs.Citas;
using Dominio.Enumeraciones;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
using CurrieTechnologies.Razor.SweetAlert2;

namespace Presentacion.Components.Pages.ECE.Citas;

public partial class ListaCitas
{
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;

    private List<CitaDTO>? Citas { get; set; }
    private bool Cargando { get; set; } = true;
    private string? MensajeError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await CargarCitas();
    }

    private async Task CargarCitas()
    {
        Cargando = true;
        MensajeError = null;
        var resultado = await CitaService.ObtenerTodosAsync();
        if (resultado.Exitoso)
        {
            Citas = resultado.Datos?.ToList() ?? new List<CitaDTO>();
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al cargar citas: " + resultado.Mensaje);
        }
        Cargando = false;
    }

    private void IrACrear() => NavigationManager.NavigateTo("/crear-cita");
    private void VerDetalles(int id) => NavigationManager.NavigateTo($"/detalles-cita/{id}");
    private void Editar(int id) => NavigationManager.NavigateTo($"/editar-cita/{id}");

    private async Task ConfirmarEliminar(int id, string nombrePaciente)
    {
        string mensajeConfirmacion = $"La cita de {nombrePaciente} se eliminará de forma permanente del sistema.";
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
            var resultado = await CitaService.EliminarAsync(id);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Cita eliminada correctamente.");
                await CargarCitas();
                StateHasChanged();
            }
            else
            {
                await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
            }
        }
        else
        {
            await Toastr.MsgInformacion("Acción cancelada: la cita fue conservada.");
        }
    }

    private string ObtenerMotivoCorto(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            return "—";
        return motivo.Length > 40 ? motivo.Substring(0, 40) + "..." : motivo;
    }

    private string ObtenerTextoEstado(EstadoCita estado)
    {
        return estado switch
        {
            EstadoCita.Pendiente => "Programada",
            EstadoCita.Confirmada => "Programada",
            EstadoCita.Completada => "Completada",
            EstadoCita.Cancelada => "Cancelada",
            EstadoCita.NoAsistio => "No asistió",
            _ => estado.ToString()
        };
    }

    private string ObtenerClaseEstado(EstadoCita estado)
    {
        return estado switch
        {
            EstadoCita.Completada => "bg-success",
            EstadoCita.Cancelada => "bg-danger",
            EstadoCita.NoAsistio => "bg-secondary",
            _ => "bg-primary"
        };
    }
}

