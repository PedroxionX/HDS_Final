using FluentValidation;
using Aplicacion.DTOs.Especialidades;
namespace Aplicacion.Validaciones.Especialidades;
public class ActualizarEspecialidadValidator : AbstractValidator<ActualizarEspecialidadDTO>
{
    public ActualizarEspecialidadValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido").MaximumLength(100).WithMessage("Máximo 100 caracteres");
        RuleFor(x => x.Descripcion).MaximumLength(500).WithMessage("Máximo 500 caracteres");
    }
}
