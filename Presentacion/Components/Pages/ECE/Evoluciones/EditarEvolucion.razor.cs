using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Evoluciones;
using Aplicacion.DTOs.HistoriasClinicas;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Evoluciones;

public partial class EditarEvolucion
{
    [Parameter] public int Id { get; set; }

    [Inject] private IToastrService Toastr { get; set; } = null!;

    private ActualizarEvolucionDTO? evolucionDTO = new ActualizarEvolucionDTO();
    private IEnumerable<HistoriaClinicaDTO>? Historias { get; set; }
    private IEnumerable<DoctorDTO>? Doctores { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;

        var historiasRes = await HistoriaClinicaService.ObtenerTodosAsync();
        if (historiasRes.Exitoso)
        {
            Historias = historiasRes.Datos;
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
        }
        else
        {
            MensajeError = "No se pudieron cargar los doctores. " + doctoresRes.Mensaje;
            await Toastr.MsgError(MensajeError);
        }

        var evolucionRes = await EvolucionService.ObtenerPorIdAsync(Id);
        if (evolucionRes.Exitoso && evolucionRes.Datos != null)
        {
            var evolucion = evolucionRes.Datos;
            evolucionDTO = new ActualizarEvolucionDTO
            {
                Id = evolucion.Id,
                IdHistoriaClinica = evolucion.IdHistoriaClinica,
                IdDoctor = evolucion.IdDoctor,
                Fecha = evolucion.Fecha,
                Diagnostico = evolucion.Diagnostico,
                Tratamiento = evolucion.Tratamiento,
                Notas = evolucion.Notas,
                Activo = evolucion.Activo
            };
        }
        else
        {
            MensajeError = "No se pudo cargar la informacion de la evolucion. " + (evolucionRes.Mensaje ?? "");
            await Toastr.MsgError(MensajeError);
        }

        Cargando = false;
    }

    private async Task GrabarEvolucion()
    {
        if (evolucionDTO == null)
            return;

        MensajeError = null;
        var resultado = await EvolucionService.ActualizarAsync(evolucionDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Evolucion actualizada exitosamente.");
            NavigationManager.NavigateTo("/evoluciones");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        NavigationManager.NavigateTo("/evoluciones");
    }

    private string Contador(string? texto, int max)
    {
        var actual = string.IsNullOrEmpty(texto) ? 0 : texto.Length;
        return $"{actual}/{max}";
    }
}

