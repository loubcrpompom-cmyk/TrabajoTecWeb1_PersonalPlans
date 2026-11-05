using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class ActualizarFavoriteDtoValidator : AbstractValidator<FavoriteDTO>
    {
        public ActualizarFavoriteDtoValidator()
        {
            // En actualización el ID es obligatorio
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID del favorito es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");
        }
    }
}