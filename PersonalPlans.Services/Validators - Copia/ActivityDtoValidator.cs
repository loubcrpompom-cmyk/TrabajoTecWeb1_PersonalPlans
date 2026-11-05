using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class ActivityDtoValidator : AbstractValidator<ActivityDTO>
    {
        public ActivityDtoValidator()
        {
            // Validación para el ID (solo para actualizaciones)
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0)
                .WithMessage("El ID de la actividad debe ser mayor que cero.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            // Descripción opcional: solo se valida el largo
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("La descripción no puede exceder los 1000 caracteres.");

            RuleFor(x => x.Category)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("La categoría es obligatoria.")
                .MaximumLength(100).WithMessage("La categoría no puede exceder los 100 caracteres.");

            RuleFor(x => x.CostPerPerson)
                .InclusiveBetween(0m, 1000000m)
                .WithMessage("El costo por persona debe estar entre 0 y 1,000,000.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 1440)
                .WithMessage("La duración debe estar entre 1 y 1440 minutos (24 horas).");

            RuleFor(x => x.SuitableCompanies)
                .NotEmpty()
                .WithMessage("Las compañías adecuadas son obligatorias.");
        }
    }
}