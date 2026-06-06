using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones;

public partial class CrearEvolucion
{
    [Parameter] public int? IdHistoria { get; set; }

    [Inject] private IToastrService Toastr { get; set; } = null!;

    private CrearEvolucionDTO evolucionDTO = new CrearEvolucionDTO();
    private IEnumerable<HistoriaClinicaDTO>? Historias { get; set; }
    private IEnumerable<DoctorDTO>? Doctores { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        evolucionDTO.Fecha = DateTime.Today;
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var historiasRes = await HistoriaClinicaService.ObtenerActivasAsync();
        if (historiasRes.Exitoso)
        {
            Historias = historiasRes.Datos;
            if (IdHistoria.HasValue)
            {
                var historiaEncontrada = Historias?.FirstOrDefault(h => h.Id == IdHistoria.Value);
                if (historiaEncontrada != null)
                    evolucionDTO.IdHistoriaClinica = historiaEncontrada.Id;
            }
            else if (Historias != null && Historias.Any())
            {
                evolucionDTO.IdHistoriaClinica = Historias.First().Id;
            }
        }
        else
        {
            MensajeError = "No se pudieron cargar las historias clinicas. " + historiasRes.Mensaje;
            await Toastr.MsgError(MensajeError);
        }

        var doctoresRes = await DoctorServicio.ObtenerTodosAsync();
        if (doctoresRes.Exitoso)
        {
            Doctores = doctoresRes.Datos;
            if (Doctores != null && Doctores.Any())
                evolucionDTO.IdDoctor = Doctores.First().Id;
        }
        else
        {
            MensajeError = "No se pudieron cargar los doctores. " + doctoresRes.Mensaje;
            await Toastr.MsgError(MensajeError);
        }

        Cargando = false;
    }

    private async Task GrabarEvolucion()
    {
        MensajeError = null;
        var resultado = await EvolucionService.CrearAsync(evolucionDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Evolucion creada exitosamente.");
            if (IdHistoria.HasValue)
                NavigationManager.NavigateTo($"/evoluciones/{IdHistoria.Value}");
            else
                NavigationManager.NavigateTo("/evoluciones");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        if (IdHistoria.HasValue)
            NavigationManager.NavigateTo($"/evoluciones/{IdHistoria.Value}");
        else
            NavigationManager.NavigateTo("/evoluciones");
    }

    private string Contador(string? texto, int max)
    {
        var actual = string.IsNullOrEmpty(texto) ? 0 : texto.Length;
        return $"{actual}/{max}";
    }
}

