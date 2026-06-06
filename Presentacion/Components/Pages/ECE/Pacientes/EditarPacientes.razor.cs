using Aplicacion.DTOs.Pacientes;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Aplicacion.Servicios.Implementaciones;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Presentacion.Components.Pages.ECE.Pacientes
{
    partial class EditarPacientes
    {
        [Parameter] public int id { get; set; }
        private PacienteDTO? pacienteOriginal;
        private ActualizarPacienteDTO pacienteEditar = new();

        protected override async Task OnInitializedAsync()
        {
            await CargarPaciente();
        }

        private async Task CargarPaciente()
        {
            var resultado = await PacienteService.ObtenerPorIdAsync(id);
            if (resultado.Exitoso && resultado.Datos != null)
            {
                pacienteOriginal = resultado.Datos;
// Mapear solo los campos editables al DTO de actualización
                pacienteEditar.Id = pacienteOriginal.Id;
                pacienteEditar.Nombres = pacienteOriginal.Nombres;
                pacienteEditar.Apellidos = pacienteOriginal.Apellidos;
                pacienteEditar.Telefono = pacienteOriginal.Telefono ?? string.Empty;
                pacienteEditar.Email = pacienteOriginal.Email ?? string.Empty;
                pacienteEditar.Direccion = pacienteOriginal.Direccion ?? string.Empty;
            }
            else
            {
                await Toastr.MsgInformacion("Paciente no encontrado o error al cargar.");
                Navigation.NavigateTo("/pacientes");
            }
        }

        private async Task GrabarPaciente()
        {
            var resultado = await PacienteService.ActualizarAsync(pacienteEditar);
            if (resultado.Exitoso)
            {
                await Toastr.MsgExito("Paciente actualizado exitosamente.");
                Navigation.NavigateTo("/pacientes");
            }
            else
            {
                await Toastr.MsgError("Error al actualizar: " + resultado.Mensaje);
            }
        }

        private void Cancelar() => Navigation.NavigateTo("/pacientes");
    }
}