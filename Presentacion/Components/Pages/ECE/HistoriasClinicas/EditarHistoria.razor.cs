using Aplicacion.DTOs.HistoriasClinicas;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas;

public partial class EditarHistoria
{
    [Parameter] public int Id { get; set; }

    [Inject] private IToastrService Toastr { get; set; } = null!;

    private ActualizarHistoriaDTO? historiaDTO = new ActualizarHistoriaDTO();
    private string NombrePaciente { get; set; } = string.Empty;
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var historiaRes = await HistoriaService.ObtenerPorIdAsync(Id);
        if (historiaRes.Exitoso && historiaRes.Datos != null)
        {
            var historia = historiaRes.Datos;
            historiaDTO = new ActualizarHistoriaDTO
            {
                Id = historia.Id,
                IdPaciente = historia.IdPaciente,
                FechaApertura = historia.FechaApertura,
                Alergias = historia.Alergias,
                AntecedentesFamiliares = historia.AntecedentesFamiliares,
                AntecedentesPersonales = historia.AntecedentesPersonales,
                Activo = historia.Activo
            };
            NombrePaciente = historia.NombrePaciente;
        }
        else
        {
            MensajeError = "No se pudo cargar la información de la historia clínica. " + (historiaRes.Mensaje ?? "");
            await Toastr.MsgError(MensajeError);
        }
        Cargando = false;
    }

    private async Task GrabarHistoria()
    {
        if (historiaDTO == null)
            return;

        MensajeError = null;
        var resultado = await HistoriaService.ActualizarAsync(historiaDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Historia clínica actualizada exitosamente.");
            NavigationManager.NavigateTo("/historias-clinicas");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        NavigationManager.NavigateTo("/historias-clinicas");
    }
}

