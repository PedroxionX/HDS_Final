using Aplicacion.DTOs.Evoluciones;
using FluentValidation;

namespace Aplicacion.Validaciones.Evoluciones;

public class CrearEvolucionValidator : AbstractValidator<CrearEvolucionDTO>
{
    public CrearEvolucionValidator()
    {
        RuleFor(e => e.IdHistoriaClinica)
            .GreaterThan(0).WithMessage("Debe seleccionar una historia clinica valida.");
        RuleFor(e => e.IdDoctor)
            .GreaterThan(0).WithMessage("Debe seleccionar un doctor valido.");
        RuleFor(e => e.Fecha)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha no puede ser futura.");
        RuleFor(e => e.Diagnostico)
            .NotEmpty().WithMessage("El diagnostico es obligatorio.")
            .MaximumLength(500).WithMessage("El diagnostico no puede superar 500 caracteres.");
        RuleFor(e => e.Tratamiento)
            .NotEmpty().WithMessage("El tratamiento es obligatorio.")
            .MaximumLength(500).WithMessage("El tratamiento no puede superar 500 caracteres.");
        RuleFor(e => e.Notas)
            .MaximumLength(1000).WithMessage("Las notas no pueden superar 1000 caracteres.");
    }
}

