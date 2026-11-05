using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class CrearRatingDtoValidator : AbstractValidator<RatingDTO>
    {
        public CrearRatingDtoValidator()
        {
            // Para creación, el ID no debe enviarse o debe ser 0
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de una nueva calificación.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThanOrEqualTo(1)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5)
                .WithMessage("La puntuación debe estar entre 1 y 5.");

            // CreatedAt lo asigna el servidor
        }
    }
}