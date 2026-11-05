using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class RatingDtoValidator : AbstractValidator<RatingDTO>
    {
        public RatingDtoValidator()
        {
            // Validación para el ID (solo para actualizaciones)
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0)
                .WithMessage("El ID de la calificación debe ser mayor que cero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThanOrEqualTo(1)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5)
                .WithMessage("La puntuación debe estar entre 1 y 5.");
        }
    }
}