using Localidades.Application.ViewModels.UserViewModels;
using FluentValidation;

namespace Localidades.Application.ViewModels.Validators.User;

public class UserEndpointRequestValidator : AbstractValidator<CreateLoginUserViewModel>
{
    public UserEndpointRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("É necessário informar um e-mail.")
            .EmailAddress().WithMessage("É necessário informar um e-mail válido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("É necessário informar uma senha.");
    }
}

