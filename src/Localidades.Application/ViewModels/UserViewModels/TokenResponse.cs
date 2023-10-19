namespace Localidades.Application.ViewModels.UserViewModels;

public record TokenResponse
{
    public string Email { get; init; }
    public string Token { get; init; }
    public DateTime ExpiraEm { get; init; } = DateTime.Now.AddHours(8);
}

