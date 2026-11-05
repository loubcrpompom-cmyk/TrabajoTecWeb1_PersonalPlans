using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDTO>
    {
        private static readonly string[] RolesPermitidos = { "User", "Admin" };

        public UserDtoValidator()
        {
            // Validación para el ID (solo para actualizaciones)
            RuleFor(x => x.Id)
                .GreaterThan(0).When(x => x.Id != 0)
                .WithMessage("El ID del usuario debe ser mayor que cero.");

            // Validación para Name
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            // Validación para Email
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(255).WithMessage("El correo no puede exceder los 255 caracteres.");

            // Validación para Password (opcional: solo se valida si viene informada)
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .MaximumLength(100).WithMessage("La contraseña no puede exceder los 100 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.");
        }
    }
}