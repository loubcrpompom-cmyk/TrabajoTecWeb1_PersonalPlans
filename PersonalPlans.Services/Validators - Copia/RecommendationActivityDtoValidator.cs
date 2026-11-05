using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class RecommendationActivityDtoValidator : AbstractValidator<RecommendationActivityDTO>
    {
        public RecommendationActivityDtoValidator()
        {
            // Validación para el ID (solo para actualizaciones)
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0)
                .WithMessage("El ID de la actividad de recomendación debe ser mayor que cero.");

            // Validación para RecommendationId
            RuleFor(x => x.RecommendationId)
                .GreaterThan(0)
                .WithMessage("El ID de la recomendación es obligatorio y debe ser mayor que cero.");

            // Validación para ActivityId
            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");

            // Validación para Order
            RuleFor(x => x.Order)
                .GreaterThan(0)
                .WithMessage("El orden debe ser mayor que cero.");

            // Validación para Cost
            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El costo no puede ser negativo.");

            // Validación para DurationMinutes
            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0)
                .WithMessage("La duración debe ser mayor que cero minutos.");
        }
    }
}