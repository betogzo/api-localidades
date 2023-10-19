using Localidades.Application.Configurations;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Localidades.Application.Endpoints;

public static class StateEndpoints
{
    public static IEndpointRouteBuilder ConfigureStateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/estados/", CreateState)
            .AddEndpointFilter<ValidationFilter<CreateStateViewModel>>()
            .RequireAuthorization(Roles.Admin);

        app.MapGet("/api/v1/estados", GetStates)
            .RequireAuthorization()
            .WithMetadata(new SwaggerOperationAttribute
            {
                Summary = "Retorna todos os estados ou apenas um a depender do parâmetro de busca.",
                Description = @"Apenas um parâmetro por busca. Se mais de um for informado a busca respeitará a precedência (código do estado, sigla, nome).
                                Se nenhum parâmetro for informado a resposta retornará todos os estados cadastrados.",
            });

        app.MapPatch("/api/v1/estados/{codigoUF}", UpdateState).RequireAuthorization(Roles.Admin);

        app.MapDelete("/api/v1/estados/{codigoUF}", DeleteState).RequireAuthorization(Roles.Admin);

        return app;
    }

    internal static async Task<Results<Conflict<ResultViewModel<string>>, Created<ResultViewModel<Estado>>>> CreateState(
       [FromServices] IStateRepository statesRepository,
       [FromBody] CreateStateViewModel novoEstado)
    {
        var estado = new Estado
        {
            CodigoUF = novoEstado.CodigoUF,
            SiglaUF = novoEstado.SiglaUF,
            NomeUF = novoEstado.NomeUF
        };

        var alreadyExists = await statesRepository.AlreadyExists(estado);

        if (alreadyExists) return TypedResults.Conflict(new ResultViewModel<string>("O Estado informado já está cadastrado no sistema."));

        var gravaNovoEstado = await statesRepository.Create(estado);

        return TypedResults.Created($"/estados/{estado.CodigoUF}", new ResultViewModel<Estado>(estado));
    }

    internal static async Task<Results<NotFound, Ok<ResultViewModel<List<Estado>>>>> GetStates(
    [FromServices] IStateRepository statesRepository,
    string? codigoEstado, string? siglaEstado, string? nomeEstado)
    {
        List<Estado> resultado = new();

        if (!String.IsNullOrEmpty(codigoEstado))
        {
            resultado.Add(await statesRepository.GetByCodigoUF(codigoEstado));
            if (resultado.Any()) TypedResults.Ok(new ResultViewModel<List<Estado>>(resultado));
        }
        else if (!String.IsNullOrEmpty(siglaEstado))
        {
            resultado.Add(await statesRepository.GetBySiglaUF(siglaEstado));
            if (resultado.Any()) TypedResults.Ok(new ResultViewModel<List<Estado>>(resultado));
        }
        else if (!String.IsNullOrEmpty(nomeEstado))
        {
            resultado.Add(await statesRepository.GetByNomeUF(nomeEstado));
            if (resultado.Any()) TypedResults.Ok(new ResultViewModel<List<Estado>>(resultado));
        }
        else
        {
            resultado = await statesRepository.GetAllEstados();
        }

        return resultado.Any() ? TypedResults.Ok(new ResultViewModel<List<Estado>>(resultado)) : TypedResults.NotFound();
    }

    internal static async Task<Results<NotFound, Ok<ResultViewModel<Estado>>>> UpdateState(
        [FromServices] IStateRepository statesRepository,
        string codigoUF,
        UpdateStateViewModel estadoAtualizado)
    {
        var estadoASerAtualizado = await statesRepository.GetByCodigoUF(codigoUF);
        if (estadoASerAtualizado is null) return TypedResults.NotFound();

        if (!String.IsNullOrEmpty(estadoAtualizado.SiglaUF))
            estadoASerAtualizado.SiglaUF = estadoAtualizado.SiglaUF;

        if (!String.IsNullOrEmpty(estadoAtualizado.NomeUF))
            estadoASerAtualizado.NomeUF = estadoAtualizado.NomeUF;

        estadoASerAtualizado.ModificadoEm = DateTime.Now;

        var estadoFoiAtualizado = await statesRepository.Update(estadoASerAtualizado);

        return TypedResults.Ok(new ResultViewModel<Estado>(estadoASerAtualizado));
    }

    internal static async Task<Results<NotFound, NoContent>> DeleteState(
        [FromServices] IStateRepository statesRepository,
        string codigoUF)
    {
        var estadoASerDeletado = await statesRepository.GetByCodigoUF(codigoUF);
        if (estadoASerDeletado is null) return TypedResults.NotFound();

        var estadoFoiDeletado = await statesRepository.Delete(estadoASerDeletado);

        return TypedResults.NoContent();
    }
}