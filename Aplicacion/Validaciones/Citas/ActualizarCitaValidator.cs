using Aplicacion.DTOs.Citas;
using FluentValidation;

namespace Aplicacion.Validaciones.Citas;

public class ActualizarCitaValidator : AbstractValidator<ActualizarCitaDTO>
{
    public ActualizarCitaValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("El ID de la cita es obligatorio.");
        RuleFor(c => c.IdPaciente)
            .GreaterThan(0).WithMessage("Debe seleccionar un paciente válido.");
        RuleFor(c => c.IdDoctor)
            .GreaterThan(0).WithMessage("Debe seleccionar un doctor válido.");
        RuleFor(c => c.FechaHora)
            .GreaterThan(DateTime.Now).WithMessage("La fecha y hora deben ser futuras.");
        RuleFor(c => c.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(500).WithMessage("El motivo no puede superar 500 caracteres.");
        RuleFor(c => c.Notas)
            .MaximumLength(1000).WithMessage("Las notas no pueden superar 1000 caracteres.");
    }
}
