using Aplicacion.DTOs.HistoriasClinicas;
using Aplicacion.DTOs.Pacientes;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.HistoriasClinicas;

public partial class CrearHistoria
{
    [Inject] private IToastrService Toastr { get; set; } = null!;

    private CrearHistoriaDTO historiaDTO = new CrearHistoriaDTO();
    private IEnumerable<PacienteDTO>? Pacientes { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarPacientes();
    }

    private async Task CargarPacientes()
    {
        Cargando = true;
        var pacientesRes = await PacienteService.ObtenerActivosSinHistoriaAsync();
        if (pacientesRes.Exitoso)
        {
            Pacientes = pacientesRes.Datos;
            if (Pacientes != null && Pacientes.Any())
                historiaDTO.IdPaciente = Pacientes.First().Id;
        }
        else
        {
            MensajeError = "No se pudieron cargar los pacientes. " + pacientesRes.Mensaje;
            await Toastr.MsgError(MensajeError);
        }
        Cargando = false;
    }

    private async Task GrabarHistoria()
    {
        MensajeError = null;
        var resultado = await HistoriaService.CrearAsync(historiaDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Historia clínica creada exitosamente.");
            NavigationManager.NavigateTo("/historias-clinicas");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        NavigationManager.NavigateTo("/historias-clinicas");
    }
}

