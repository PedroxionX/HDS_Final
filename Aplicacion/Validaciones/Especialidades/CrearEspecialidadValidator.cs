using FluentValidation;
using Aplicacion.DTOs.Especialidades;
namespace Aplicacion.Validaciones.Especialidades;
public class CrearEspecialidadValidator : AbstractValidator<CrearEspecialidadDTO>
{
    public CrearEspecialidadValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido").MaximumLength(100).WithMessage("Máximo 100 caracteres");
        RuleFor(x => x.Descripcion).MaximumLength(500).WithMessage("Máximo 500 caracteres");
    }
}
