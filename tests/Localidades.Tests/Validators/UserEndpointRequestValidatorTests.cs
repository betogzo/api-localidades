using Localidades.Application.ViewModels.UserViewModels;
using Localidades.Application.ViewModels.Validators.User;

namespace Localidades.Tests.Validators;

public class UserEndpointRequestValidatorTests
{
    private readonly UserEndpointRequestValidator _sut = new();

    [Fact]
    public void Invalid_WhenEmailIsEmpty()
    {
        var model = new CreateLoginUserViewModel { Email = "", Senha = "x" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateLoginUserViewModel.Email));
    }

    [Fact]
    public void Invalid_WhenEmailIsNotValidFormat()
    {
        var model = new CreateLoginUserViewModel { Email = "nao-e-email", Senha = "x" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateLoginUserViewModel.Email));
    }

    [Fact]
    public void Invalid_WhenPasswordIsEmpty()
    {
        var model = new CreateLoginUserViewModel { Email = "a@b.com", Senha = "" };

        var r = _sut.Validate(model);

        Assert.False(r.IsValid);
        Assert.Contains(r.Errors, e => e.PropertyName == nameof(CreateLoginUserViewModel.Senha));
    }

    [Fact]
    public void Valid_WhenEmailAndPasswordAreFilled()
    {
        var model = new CreateLoginUserViewModel { Email = "a@b.com", Senha = "secret" };

        var r = _sut.Validate(model);

        Assert.True(r.IsValid);
    }
}
