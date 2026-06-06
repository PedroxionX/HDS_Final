using Aplicacion.DTOs.HistoriasClinicas;
using FluentValidation;

namespace Aplicacion.Validaciones.HistoriasClinicas;

public class ActualizarHistoriaValidator : AbstractValidator<ActualizarHistoriaDTO>
{
    public ActualizarHistoriaValidator()
    {
        RuleFor(h => h.Id)
            .GreaterThan(0).WithMessage("El ID de la historia clínica es obligatorio.");
        RuleFor(h => h.IdPaciente)
            .GreaterThan(0).WithMessage("Debe seleccionar un paciente válido.");
        RuleFor(h => h.FechaApertura)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de apertura no puede ser futura.");
        RuleFor(h => h.Alergias)
            .MaximumLength(500).WithMessage("Las alergias no pueden superar 500 caracteres.");
        RuleFor(h => h.AntecedentesFamiliares)
            .MaximumLength(500).WithMessage("Los antecedentes familiares no pueden superar 500 caracteres.");
        RuleFor(h => h.AntecedentesPersonales)
            .MaximumLength(500).WithMessage("Los antecedentes personales no pueden superar 500 caracteres.");
    }
}

