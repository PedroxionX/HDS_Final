using FluentValidation;
using Aplicacion.DTOs.Doctores;
namespace Aplicacion.Validaciones.Doctores;
public class CrearDoctorValidator : AbstractValidator<CrearDoctorDTO>
{
    public CrearDoctorValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");
        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El email no es válido.");
        RuleFor(x => x.FechaContratacion)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de contratación no puede ser futura.");
        RuleFor(x => x.IdEspecialidad)
            .GreaterThan(0).WithMessage("Debe seleccionar una especialidad.");
    }
}
