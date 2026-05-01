using Localidades.Application.Endpoints;
using Localidades.Application.Services;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Application.ViewModels.UserViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Localidades.Tests.Fixtures;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Localidades.Tests.ServiceTests;

[Collection("Jwt")]
public class UserEndpointsTests
{
    [Fact]
    public async Task CreateUser_ReturnsConflict_WhenEmailAlreadyRegistered()
    {
        var users = new Mock<IUsersRepository>();
        users.Setup(r => r.AlreadyExistsByEmail("dup@email.com")).ReturnsAsync(true);
        var hasher = new PasswordHasherService();
        var body = new CreateLoginUserViewModel { Email = "dup@email.com", Senha = "senha123" };

        var result = await UserEndpoints.CreateUser(hasher, users.Object, body);

        Assert.IsType<Conflict<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreated_WhenEmailIsNew()
    {
        var users = new Mock<IUsersRepository>();
        users.Setup(r => r.AlreadyExistsByEmail(It.IsAny<string>())).ReturnsAsync(false);
        users.Setup(r => r.Create(It.IsAny<Usuario>())).ReturnsAsync(true);
        var hasher = new PasswordHasherService();
        var body = new CreateLoginUserViewModel { Email = "novo@email.com", Senha = "senha123" };

        var result = await UserEndpoints.CreateUser(hasher, users.Object, body);

        Assert.IsType<Created<Usuario>>(result.Result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        var users = new Mock<IUsersRepository>();
        users.Setup(r => r.GetByEmail("ghost@test.com")).Returns(Task.FromResult<Usuario>(null!));
        var hasher = new PasswordHasherService();
        var tokens = new TokenService();
        var body = new CreateLoginUserViewModel { Email = "ghost@test.com", Senha = "x" };

        var result = await UserEndpoints.AuthenticateUser(hasher, tokens, users.Object, body);

        Assert.IsType<UnauthorizedHttpResult>(result.Result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        var users = new Mock<IUsersRepository>();
        var hasher = new PasswordHasherService();
        var storedHash = hasher.Hash("certa");
        users.Setup(r => r.GetByEmail("u@test.com"))
            .ReturnsAsync(new Usuario { Email = "u@test.com", Senha = storedHash });
        var tokens = new TokenService();
        var body = new CreateLoginUserViewModel { Email = "u@test.com", Senha = "errada" };

        var result = await UserEndpoints.AuthenticateUser(hasher, tokens, users.Object, body);

        Assert.IsType<UnauthorizedHttpResult>(result.Result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnsOkWithToken_WhenCredentialsAreValid()
    {
        var users = new Mock<IUsersRepository>();
        var hasher = new PasswordHasherService();
        var storedHash = hasher.Hash("certa");
        users.Setup(r => r.GetByEmail("ok@test.com"))
            .ReturnsAsync(new Usuario { Email = "ok@test.com", Senha = storedHash });
        var tokens = new TokenService();
        var body = new CreateLoginUserViewModel { Email = "ok@test.com", Senha = "certa" };

        var result = await UserEndpoints.AuthenticateUser(hasher, tokens, users.Object, body);

        var ok = Assert.IsType<Ok<ResultViewModel<TokenResponse>>>(result.Result);
        Assert.NotNull(ok.Value);
        Assert.NotNull(ok.Value.Data);
        Assert.False(string.IsNullOrWhiteSpace(ok.Value.Data.Token));
        Assert.Equal("ok@test.com", ok.Value.Data.Email);
    }
}
