namespace Localidades.Application.ViewModels.StateViewModels;

public record CreateStateViewModel
{
    public string CodigoUF { get; init; }
    public string SiglaUF { get; init; }
    public string NomeUF { get; init; }
}

