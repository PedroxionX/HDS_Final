using Aplicacion.DTOs.Citas;
using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Helpers;
using Dominio.Enumeraciones;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;

namespace Presentacion.Components.Pages.ECE.Citas;

public partial class CrearCita
{
    [Inject] private IToastrService Toastr { get; set; } = null!;

    private CrearCitaDTO citaDTO = new CrearCitaDTO();
    private IEnumerable<PacienteDTO>? Pacientes { get; set; }
    private IEnumerable<DoctorDTO>? Doctores { get; set; }
    private List<(EstadoCita Value, string Nombre)> Estados { get; } = EnumHelper.ObtenerOpcionesEnum<EstadoCita>();
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;

    private DateTime _fechaSeleccionada = DateTime.Today.AddDays(1);
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
        SincronizarFechaHora();
    }

    private async Task CargarDatos()
    {
        Cargando = true;
        var pacientesRes = await PacienteService.ObtenerTodosAsync();
        if (pacientesRes.Exitoso)
        {
            Pacientes = pacientesRes.Datos;
            if (Pacientes != null && Pacientes.Any())
                citaDTO.IdPaciente = Pacientes.First().Id;
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
            if (Doctores != null && Doctores.Any())
                citaDTO.IdDoctor = Doctores.First().Id;
        }
        else
        {
            MensajeError = "No se pudieron cargar los doctores. " + doctoresRes.Mensaje;
            await Toastr.MsgError(MensajeError);
        }
        Cargando = false;
    }

    private void SincronizarFechaHora()
    {
        citaDTO.FechaHora = _fechaSeleccionada.Date.Add(_horaSeleccionada.ToTimeSpan());
    }

    private async Task GrabarCita()
    {
        MensajeError = null;
        SincronizarFechaHora();
        var resultado = await CitaService.CrearAsync(citaDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Cita creada exitosamente.");
            NavigationManager.NavigateTo("/citas");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
        }
    }

    private void Cancelar()
    {
        NavigationManager.NavigateTo("/citas");
    }
}
