using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Aplicacion.DTOs.Pacientes;
using Aplicacion.Servicios.Interfaces;
using Presentacion.Servicios;
using CurrieTechnologies.Razor.SweetAlert2;

namespace Presentacion.Components.Pages.ECE.Pacientes
{
    public partial class Pacientes : ComponentBase
    {
// Inyección de dependencias, asegurando que las propiedades sean no nulas,
// para evitar errores en tiempo de ejecución.
        [Inject] private IPacienteService serviciosPaciente { get; set; } = null!;
        [Inject] private IToastrService Toastr { get; set; } = null!;
        [Inject] private SweetAlertService Swal { get; set; } = null!;

        [Inject] private NavigationManager Navigation { get; set; } = null!;

// Propiedad para controlar el estilo de visualización (card o tabla)
        protected bool EstiloCard = true;
        protected List<PacienteDTO>? pacientes;

        protected override async Task OnInitializedAsync()
        {
            await CargarPacientes();
        }

        protected async Task CargarPacientes()
        {
            var resultado = await serviciosPaciente.ObtenerTodosAsync();
            if (resultado.Exitoso && resultado.Datos != null)
            {
                pacientes = resultado.Datos.ToList();
            }
            else
            {
                await Toastr.MsgError("Error al cargar los pacientes: " + resultado.Mensaje);
                pacientes = new List<PacienteDTO>();
            }
        }

        protected void CambiaEstilo()
        {
            EstiloCard = !EstiloCard;
        }

// Métodos de navegación para crear, ver detalles y editar pacientes
        protected void NavegarACrear() => Navigation.NavigateTo("/crear-paciente");
        protected void VerDetalles(int id) => Navigation.NavigateTo($"/detalles-paciente/{id}");
        protected void Editar(int id) => Navigation.NavigateTo($"/editar-paciente/{id}");

        protected async Task ConfirmarEliminar(int id, string nombres, string apellidos)
        {
// Construir el mensaje incluyendo el nombre completo del paciente
            string nombreCompleto = $"{nombres} {apellidos}";
            string mensajeConfirmacion =
                $"Los datos del paciente: {nombreCompleto} se eliminarán de forma permanente del sistemas.";
            
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
                var resultado = await serviciosPaciente.EliminarAsync(id);
                if (resultado.Exitoso)
                {
                    await Toastr.MsgExito($"Paciente {nombreCompleto} eliminado correctamente.");
                    await CargarPacientes();
                    StateHasChanged();
                }
                else
                {
                    await Toastr.MsgError("Error al eliminar: " + resultado.Mensaje);
                }
            }
            else
            {
                await Toastr.MsgInformacion(
                    $"Acción cancelada: Los datos del paciente {nombreCompleto} fueron conservados.");
            }
        }
    }
}