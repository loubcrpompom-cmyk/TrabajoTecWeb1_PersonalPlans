using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class FavoriteDtoValidator : AbstractValidator<FavoriteDTO>
    {
        public FavoriteDtoValidator()
        {
            // Validación para el ID (solo para actualizaciones)
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0)
                .WithMessage("El ID del favorito debe ser mayor que cero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");
        }
    }
}