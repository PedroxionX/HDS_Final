using Aplicacion.DTOs.Citas;
using Dominio.Enumeraciones;
using Microsoft.AspNetCore.Components;

namespace Presentacion.Components.Pages.ECE.Citas;

public partial class DetalleCita
{
    [Parameter] public int Id { get; set; }

    private CitaDTO? Cita { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var resultado = await CitaService.ObtenerPorIdAsync(Id);
        if (resultado.Exitoso)
        {
            Cita = resultado.Datos;
        }
        else
        {
            MensajeError = "No se pudo cargar la información de la cita. " + resultado.Mensaje;
        }
        Cargando = false;
    }

    private void VolverAlListado()
    {
        NavigationManager.NavigateTo("/citas");
    }

    private void EditarCita(int citaId)
    {
        NavigationManager.NavigateTo($"/editar-cita/{citaId}");
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
}

