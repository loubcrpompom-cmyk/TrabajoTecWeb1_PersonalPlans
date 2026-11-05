using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class CrearRecommendationDtoValidator : AbstractValidator<RecommendationDTO>
    {
        public CrearRecommendationDtoValidator()
        {
            // Para creación, el ID no debe enviarse o debe ser 0
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de una nueva recomendación.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Budget)
                .InclusiveBetween(0m, 10000m)
                .WithMessage("El presupuesto debe estar entre 0 y 10000.");

            RuleFor(x => x.AvailableTimeHours)
                .InclusiveBetween(1, 12)
                .WithMessage("El tiempo disponible debe estar entre 1 y 12 horas.");

            RuleFor(x => x.Company)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("La compañía es obligatoria.")
                .MaximumLength(100).WithMessage("La compañía no puede exceder los 100 caracteres.");

            RuleFor(x => x.NumberOfPeople)
                .InclusiveBetween(1, 10)
                .WithMessage("El número de personas debe estar entre 1 y 10.");

            // TotalCost, TotalDurationMinutes y CreatedAt los calcula/asigna el servidor
        }
    }
}