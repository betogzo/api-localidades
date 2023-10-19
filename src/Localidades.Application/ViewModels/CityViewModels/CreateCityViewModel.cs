namespace Localidades.Application.ViewModels.CityViewModels;

public record CreateCityViewModel
{
    public string CodigoIBGE { get; init; }
    public string CodigoUF { get; init; }
    public string NomeMunicipio { get; init; }
}