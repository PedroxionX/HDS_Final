using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Helpers;
using Dominio.Enumeraciones;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Citas;

public partial class EditarCita
{
    [Parameter] public int Id { get; set; }

    [Inject] private IToastrService Toastr { get; set; } = null!;

    private ActualizarCitaDTO? citaDTO = new ActualizarCitaDTO();
    private IEnumerable<PacienteDTO>? Pacientes { get; set; }
    private IEnumerable<DoctorDTO>? Doctores { get; set; }
    private List<(EstadoCita Value, string Nombre)> Estados { get; } = EnumHelper.ObtenerOpcionesEnum<EstadoCita>();
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    private DateTime _fechaSeleccionada = DateTime.Today;
    private TimeOnly _horaSeleccionada = new TimeOnly(9, 0);

    private DateTime FechaSeleccionada
    {
        get => _fechaSeleccionada;
        set
        {
            _fechaSeleccionada = value;
            SincronizarFechaHora();
        }
    }

    private TimeOnly HoraSeleccionada
    {
        get => _horaSeleccionada;
        set
        {
            _horaSeleccionada = value;
            SincronizarFechaHora();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        Cargando = true;

        var pacientesRes = await PacienteService.ObtenerTodosAsync();
        if (pacientesRes.Exitoso)
        {
            Pacientes = pacientesRes.Datos;
        }
        else
        {
            MensajeError = "No se pudieron cargar los pacientes. " + pacientesRes.Mensaje;
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

        var citaRes = await CitaService.ObtenerPorIdAsync(Id);
        if (citaRes.Exitoso && citaRes.Datos != null)
        {
            var cita = citaRes.Datos;
            citaDTO = new ActualizarCitaDTO
            {
                Id = cita.Id,
                IdPaciente = cita.IdPaciente,
                IdDoctor = cita.IdDoctor,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo,
                Notas = cita.Notas,
                Estado = cita.Estado
            };

            _fechaSeleccionada = cita.FechaHora.Date;
            _horaSeleccionada = TimeOnly.FromDateTime(cita.FechaHora);
        }
        else
        {
            MensajeError = "No se pudo cargar la información de la cita. " + (citaRes.Mensaje ?? "");
            await Toastr.MsgError(MensajeError);
        }

        Cargando = false;
    }

    private void SincronizarFechaHora()
    {
        if (citaDTO != null)
            citaDTO.FechaHora = _fechaSeleccionada.Date.Add(_horaSeleccionada.ToTimeSpan());
    }

    private async Task GrabarCita()
    {
        if (citaDTO == null)
            return;

        MensajeError = null;
        SincronizarFechaHora();
        var resultado = await CitaService.ActualizarAsync(citaDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Cita actualizada exitosamente.");
            NavigationManager.NavigateTo("/citas");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        NavigationManager.NavigateTo("/citas");
    }
}

