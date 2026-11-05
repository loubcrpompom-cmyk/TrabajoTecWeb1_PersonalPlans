using FluentValidation;
using PersonalPlans.Core.DTOs;

namespace PersonalPlans.Services.Validators
{
    public class CrearUserDtoValidator : AbstractValidator<UserDTO>
    {
        private static readonly string[] RolesPermitidos = { "User", "Admin" };

        public CrearUserDtoValidator()
        {
            // Para creación, el ID no debe enviarse o debe ser 0
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de un nuevo usuario.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(255).WithMessage("El correo no puede exceder los 255 caracteres.");

            // En creación la contraseña es OBLIGATORIA
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .MaximumLength(100).WithMessage("La contraseña no puede exceder los 100 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.");

            RuleFor(x => x.Role)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El rol es obligatorio.")
                .Must(r => RolesPermitidos.Contains(r))
                .WithMessage("El rol debe ser 'User' o 'Admin'.");

            // CreatedAt e IsActive no se validan: los asigna el servidor al crear
        }
    }
}