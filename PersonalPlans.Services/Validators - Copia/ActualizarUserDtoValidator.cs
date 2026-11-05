using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class ActualizarUserDtoValidator : AbstractValidator<UserDTO>
    {
        private static readonly string[] RolesPermitidos = { "User", "Admin" };

        public ActualizarUserDtoValidator()
        {
            // En actualización el Id es obligatorio
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(255).WithMessage("El correo no puede exceder los 255 caracteres.");

            // Password es opcional al actualizar: solo se valida si viene informada
            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .MaximumLength(50).WithMessage("La contraseña no puede exceder los 50 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("El rol es obligatorio.")
                .Must(r => RolesPermitidos.Contains(r))
                .WithMessage("El rol debe ser 'User' o 'Admin'.");

            // CreatedAt es DateTime (no string): no se compara con TryParse
            RuleFor(x => x.CreatedAt)
                .NotEqual(default(DateTime)).WithMessage("La fecha de creación es obligatoria.")
                .LessThanOrEqualTo(_ => DateTime.UtcNow.AddMinutes(5))
                .WithMessage("La fecha de creación no puede estar en el futuro.");
        }
    }
}