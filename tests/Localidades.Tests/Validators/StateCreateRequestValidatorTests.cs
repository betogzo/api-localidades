using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Application.ViewModels.Validators.State;

namespace Localidades.Tests.Validators;

public class StateCreateRequestValidatorTests
{
    private readonly StateCreateRequestValidator _sut = new();

    [Fact]
    public void Invalid_WhenCodigoUFIsEmpty()
    {
        var model = new CreateStateViewModel { CodigoUF = "", SiglaUF = "SP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenCodigoUFNotNumeric()
    {
        var model = new CreateStateViewModel { CodigoUF = "AB", SiglaUF = "SP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenCodigoUFLengthWrong()
    {
        var model = new CreateStateViewModel { CodigoUF = "351", SiglaUF = "SP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.CodigoUF));
    }

    [Fact]
    public void Invalid_WhenSiglaUFIsEmpty()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.SiglaUF));
    }

    [Fact]
    public void Invalid_WhenSiglaUFContainsDigits()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "S1", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.SiglaUF));
    }

    [Fact]
    public void Invalid_WhenSiglaUFLengthWrong()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SPP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.SiglaUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFIsEmpty()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SP", NomeUF = "" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.NomeUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFContainsDigits()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SP", NomeUF = "S4o Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.NomeUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFTooShort()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SP", NomeUF = "Rio" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.NomeUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFTooLong()
    {
        var nome = new string('a', 51);
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SP", NomeUF = nome };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateStateViewModel.NomeUF));
    }

    [Fact]
    public void Valid_WhenAllFieldsCorrect()
    {
        var model = new CreateStateViewModel { CodigoUF = "35", SiglaUF = "SP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }
}
