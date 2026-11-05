using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class ActualizarRecommendationActivityDtoValidator : AbstractValidator<RecommendationActivityDTO>
    {
        public ActualizarRecommendationActivityDtoValidator()
        {
            // En actualización el ID es obligatorio
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad de recomendación es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.RecommendationId)
                .GreaterThan(0)
                .WithMessage("El ID de la recomendación es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Order)
                .GreaterThan(0)
                .WithMessage("El orden debe ser mayor que cero.");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El costo no puede ser negativo.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0)
                .WithMessage("La duración debe ser mayor que cero minutos.");
        }
    }
}