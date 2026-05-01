using Localidades.Application.ViewModels.CityViewModels;
using Localidades.Application.ViewModels.Validators.City;

namespace Localidades.Tests.Validators;

public class CityUpdateRequestValidatorTests
{
    private readonly CityUpdateRequestValidator _sut = new();

    [Fact]
    public void Valid_WhenCodigoIBGEIsNull()
    {
        var model = new UpdateCityViewModel { CodigoIBGE = null, NomeMunicipio = "Santos" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }

    [Fact]
    public void Valid_WhenCodigoIBGEIsWhitespace()
    {
        var model = new UpdateCityViewModel { CodigoIBGE = "   ", NomeMunicipio = "Santos" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }

    [Fact]
    public void Invalid_WhenCodigoIBGEIsNotNumeric()
    {
        var model = new UpdateCityViewModel { CodigoIBGE = "ABCDEFG" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateCityViewModel.CodigoIBGE));
    }

    [Fact]
    public void Invalid_WhenCodigoIBGELengthWrong()
    {
        var model = new UpdateCityViewModel { CodigoIBGE = "12345" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateCityViewModel.CodigoIBGE));
    }

    [Fact]
    public void Valid_WhenCodigoIBGECorrect()
    {
        var model = new UpdateCityViewModel { CodigoIBGE = "3550308" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }
}
