using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class CrearFavoriteDtoValidator : AbstractValidator<FavoriteDTO>
    {
        public CrearFavoriteDtoValidator()
        {
            // Para creación, el ID no debe enviarse o debe ser 0
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de un nuevo favorito.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0)
                .WithMessage("El ID de la actividad es obligatorio y debe ser mayor que cero.");

            // CreatedAt lo asigna el servidor; Activity es solo de lectura
        }
    }
}