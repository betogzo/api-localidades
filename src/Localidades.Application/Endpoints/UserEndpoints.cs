using Localidades.Application.Services;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Application.ViewModels.UserViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Localidades.Application.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder ConfigureUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/usuarios/registrar", CreateUser).AddEndpointFilter<ValidationFilter<CreateLoginUserViewModel>>(); ;

        app.MapPost("/api/v1/usuarios/autenticar", AuthenticateUser).AddEndpointFilter<ValidationFilter<CreateLoginUserViewModel>>();

        return app;
    }

    internal static async Task<Results<Conflict<ResultViewModel<string>>, Created<Usuario>>> CreateUser(
        [FromServices] PasswordHasherService passwordHasher,
        [FromServices] IUsersRepository usersRepository,
        [FromBody] CreateLoginUserViewModel novoUsuario)
    {
        var usuarioJaExiste = await usersRepository.AlreadyExistsByEmail(novoUsuario.Email);
        if (usuarioJaExiste) return TypedResults.Conflict(new ResultViewModel<string>("Já existe um usuário cadastrado com o e-mail informado."));

        var usuario = new Usuario
        {
            Email = novoUsuario.Email,
            Senha = passwordHasher.Hash(novoUsuario.Senha)
        };

        var gravaNovoUsuario = await usersRepository.Create(usuario);

        return TypedResults.Created($"/usuarios/{usuario.Id}", usuario);
    }

    internal static async Task<Results<UnauthorizedHttpResult, Ok<ResultViewModel<TokenResponse>>>> AuthenticateUser(
        [FromServices] PasswordHasherService passwordHasher,
        [FromServices] TokenService tokenService,
        [FromServices] IUsersRepository usersRepository,
        CreateLoginUserViewModel credencial)
    {
        var usuario = await usersRepository.GetByEmail(credencial.Email);
        var senhaConfere = usuario is not null && passwordHasher.Verify(usuario.Senha, credencial.Senha);

        if (!senhaConfere) return TypedResults.Unauthorized();

        return TypedResults.Ok(new ResultViewModel<TokenResponse>(new TokenResponse { Email = usuario!.Email, Token = tokenService.GenerateToken(usuario) }));
    }
}