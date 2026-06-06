using FluentValidation;
using Aplicacion.DTOs.Doctores;
namespace Aplicacion.Validaciones.Doctores;
public class ActualizarDoctorValidator : AbstractValidator<ActualizarDoctorDTO>
{
    public ActualizarDoctorValidator()
    {
        Include(new CrearDoctorValidator());
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del doctor es obligatorio.");
    }
}
