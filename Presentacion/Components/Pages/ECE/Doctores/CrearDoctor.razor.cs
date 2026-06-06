using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Doctores;
public partial class CrearDoctor
{
    [Inject] private IToastrService Toastr { get; set; } = null!;
    private CrearDoctorDTO doctorDTO = new CrearDoctorDTO();
    private IEnumerable<EspecialidadDTO>? Especialidades { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;
    protected override async Task OnInitializedAsync()
    {
        await CargarEspecialidades();
    }
    private async Task CargarEspecialidades()
    {
        Cargando = true;
        var resultado = await EspecialidadService.ObtenerTodosAsync();
        if (resultado.Exitoso)
        {
            Especialidades = resultado.Datos;
            if (Especialidades != null && Especialidades.Any())
            {
                doctorDTO.IdEspecialidad = Especialidades.First().Id;
            }
        }
        else
        {
            MensajeError = "No se pudieron cargar las especialidades. " + resultado.Mensaje;
            await Toastr.MsgError(MensajeError);
        }
        Cargando = false;
    }
    private async Task GrabarDoctor()
    {
        MensajeError = null;
        var resultado = await DoctorServicio.CrearAsync(doctorDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Doctor creado exitosamente.");
            NavigationManager.NavigateTo("/doctores");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al crear: " + resultado.Mensaje);
        }
    }
    private void Cancelar()
    {
        NavigationManager.NavigateTo("/doctores");
    }
}
