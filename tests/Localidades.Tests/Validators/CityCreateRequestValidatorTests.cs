using Localidades.Application.ViewModels.CityViewModels;
using Localidades.Application.ViewModels.Validators.City;

namespace Localidades.Tests.Validators;

public class CityCreateRequestValidatorTests
{
    private readonly CityCreateRequestValidator _sut = new();

    [Fact]
    public void Invalid_WhenCodigoUFIsEmpty()
    {
        var model = new CreateCityViewModel { CodigoUF = "", CodigoIBGE = "3550308", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenCodigoUFIsNotNumeric()
    {
        var model = new CreateCityViewModel { CodigoUF = "AB", CodigoIBGE = "3550308", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenCodigoUFLengthWrong()
    {
        var model = new CreateCityViewModel { CodigoUF = "351", CodigoIBGE = "3550308", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenCodigoIBGEIsEmpty()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoIBGE));
    }

    [Fact]
    public void Invalid_WhenCodigoIBGEIsNotNumeric()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "ABCDEFG", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoIBGE));
    }

    [Fact]
    public void Invalid_WhenCodigoIBGELengthWrong()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "12345", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.CodigoIBGE));
    }

    [Fact]
    public void Invalid_WhenNomeMunicipioIsEmpty()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "3550308", NomeMunicipio = "" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.NomeMunicipio));
    }

    [Fact]
    public void Invalid_WhenNomeMunicipioContainsDigits()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "3550308", NomeMunicipio = "S4ntos" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.NomeMunicipio));
    }

    [Fact]
    public void Invalid_WhenNomeMunicipioTooShort()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "3550308", NomeMunicipio = "Rio" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.NomeMunicipio));
    }

    [Fact]
    public void Invalid_WhenNomeMunicipioTooLong()
    {
        var nome = new string('a', 81);
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "3550308", NomeMunicipio = nome };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateCityViewModel.NomeMunicipio));
    }

    [Fact]
    public void Valid_WhenAllFieldsCorrect()
    {
        var model = new CreateCityViewModel { CodigoUF = "35", CodigoIBGE = "3550308", NomeMunicipio = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }
}
