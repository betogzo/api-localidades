using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Application.ViewModels.Validators.State;

namespace Localidades.Tests.Validators;

public class StateUpdateRequestValidatorTests
{
    private readonly StateEndpointRequestValidator _sut = new();

    [Fact]
    public void Invalid_WhenSiglaUFContainsDigits()
    {
        var model = new UpdateStateViewModel { SiglaUF = "S1", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateStateViewModel.SiglaUF));
    }

    [Fact]
    public void Invalid_WhenSiglaUFLengthWrong()
    {
        var model = new UpdateStateViewModel { SiglaUF = "SPP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateStateViewModel.SiglaUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFTooShort()
    {
        var model = new UpdateStateViewModel { SiglaUF = "SP", NomeUF = "Rio" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateStateViewModel.NomeUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFContainsDigits()
    {
        var model = new UpdateStateViewModel { SiglaUF = "SP", NomeUF = "S4o Paulo" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateStateViewModel.NomeUF));
    }

    [Fact]
    public void Invalid_WhenNomeUFTooLong()
    {
        var nome = new string('a', 51);
        var model = new UpdateStateViewModel { SiglaUF = "SP", NomeUF = nome };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(UpdateStateViewModel.NomeUF));
    }

    [Fact]
    public void Valid_WhenAllFieldsCorrect()
    {
        var model = new UpdateStateViewModel { SiglaUF = "SP", NomeUF = "São Paulo" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }
}
