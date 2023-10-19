namespace Localidades.Application.ViewModels.CityViewModels;

public record GetCityResponseViewModel
{
    public string CodigoIBGE { get; init; }
    public string NomeMunicipio { get; init; }
    public string SiglaEstado { get; init; }
    public string NomeEstado { get; init; }
}