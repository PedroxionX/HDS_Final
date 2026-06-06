using Aplicacion.DTOs.Doctores;
using Aplicacion.DTOs.Especialidades;
using Microsoft.AspNetCore.Components;
using Presentacion.Servicios;
namespace Presentacion.Components.Pages.ECE.Doctores;
public partial class EditarDoctor
{
    [Inject] private IToastrService Toastr { get; set; } = null!;
    [Parameter]
    public int id { get; set; }
    private ActualizarDoctorDTO doctorDTO = new ActualizarDoctorDTO();
    private IEnumerable<EspecialidadDTO>? Especialidades { get; set; }
    private string? MensajeError { get; set; }
    private bool Cargando { get; set; } = true;
    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }
    private async Task CargarDatos()
    {
        Cargando = true;
        var especialidadesRes = await EspecialidadService.ObtenerTodosAsync();
        if (especialidadesRes.Exitoso)
        {
            Especialidades = especialidadesRes.Datos;
        }
        var doctorRes = await DoctorServicio.ObtenerPorIdAsync(id);
        if (doctorRes.Exitoso && doctorRes.Datos != null)
        {
            var doc = doctorRes.Datos;
            doctorDTO = new ActualizarDoctorDTO
            {
                Id = doc.Id,
                Nombre = doc.Nombre,
                Descripcion = doc.Descripcion,
                IdEspecialidad = doc.IdEspecialidad,
                Telefono = doc.Telefono,
                Email = doc.Email,
                FechaContratacion = doc.FechaContratacion,
                Activo = doc.Activo
            };
        }
        else
        {
            MensajeError = "No se pudo cargar la información del doctor. " + (doctorRes.Mensaje ?? "");
            await Toastr.MsgError(MensajeError);
        }
        Cargando = false;
    }
    private async Task GrabarDoctor()
    {
        MensajeError = null;
        var resultado = await DoctorServicio.ActualizarAsync(doctorDTO);
        if (resultado.Exitoso)
        {
            await Toastr.MsgExito("Doctor actualizado exitosamente.");
            NavigationManager.NavigateTo("/doctores");
        }
        else
        {
            MensajeError = resultado.Mensaje;
            await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
        }
    }
    private void Cancelar()
    {
        NavigationManager.NavigateTo("/doctores");
    }
}
