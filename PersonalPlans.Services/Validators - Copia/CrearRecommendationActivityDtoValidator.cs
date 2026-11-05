using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class CrearRecommendationActivityDtoValidator : AbstractValidator<RecommendationActivityDTO>
    {
        public CrearRecommendationActivityDtoValidator()
        {
            // Para creación, el ID no debe enviarse o debe ser 0
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de una nueva actividad de recomendación.");

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

            // Activity es solo de lectura (navegación): no se valida en creación
        }
    }
}