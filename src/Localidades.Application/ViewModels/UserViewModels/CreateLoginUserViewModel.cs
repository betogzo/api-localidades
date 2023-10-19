namespace Localidades.Application.ViewModels.UserViewModels;

public record CreateLoginUserViewModel
{
    public string Email { get; init; }
    public string Senha { get; init; }
}

