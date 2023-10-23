using Localidades.Application.Configurations;
using Localidades.Application.ViewModels.CityViewModels;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Localidades.Application.Endpoints;

public static class CityEndpoints
{
    public static IEndpointRouteBuilder ConfigureCityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/municipios", CreateCity)
            .AddEndpointFilter<ValidationFilter<CreateCityViewModel>>()
            .RequireAuthorization(Roles.Admin);

        app.MapGet("/api/v1/municipios", GetCities)
            .RequireAuthorization()
            .WithMetadata(new SwaggerOperationAttribute
            {
                Summary = "Retorna todos os municípios ou apenas um a depender do parâmetro de busca.",
                Description = @"Os únicos parâmetros que podem ser utilizados simultaneamente são NomeMunicipio e SiglaEstado. Se outra combinação for utilizada,
                                a busca irá trazer resultado(s) pela seguinte precedência: codigoIBGE, siglaEstado, nomeMunicipio. Se nenhum parâmetro for informado,
                                o response retornará todos os municípios cadastrados. Esta rota retorna dados paginados. Valores aceitos: Take = min.: 0 máx.: 250 / Skip = min.: 0",
            });

        app.MapPatch("/api/v1/municipios/{codigoIBGE}", UpdateCity)
            .AddEndpointFilter<ValidationFilter<UpdateCityViewModel>>()
            .RequireAuthorization(Roles.Admin);

        app.MapDelete("/api/v1/municipios/{codigoIBGE}", DeleteCity)
            .RequireAuthorization(Roles.Admin);

        return app;
    }

    public static async Task<Results<Conflict<ResultViewModel<string>>, Created<ResultViewModel<Municipio>>>> CreateCity(
        [FromServices] ICityRepository citiesRepository,
        [FromServices] IStateRepository statesRepository,
        CreateCityViewModel novoMunicipio
        )
    {
        var estadoExiste = await statesRepository.GetByCodigoUF(novoMunicipio.CodigoUF);

        if (estadoExiste is null) 
            return TypedResults.Conflict(new ResultViewModel<string>("O código de Estado informado não corresponde a nenhum Estado cadastrado na base de dados."));

        var municipio = new Municipio
        {
            CodigoIBGE = novoMunicipio.CodigoIBGE,
            CodigoUF = novoMunicipio.CodigoUF,
            NomeMunicipio = novoMunicipio.NomeMunicipio,
        };

        var alreadyExists = await citiesRepository.AlreadyExists(municipio);
        if (alreadyExists) return TypedResults.Conflict(new ResultViewModel<string>("Cidade já cadastrada no sistema."));

        var cidadeCadastrada = await citiesRepository.Create(municipio);

        return TypedResults.Created("/usuarios", new ResultViewModel<Municipio>(municipio));
    }

    public static async Task<Results<
        NotFound, 
        BadRequest<ResultViewModel<string>>, 
        Ok<PagedResultViewModel<List<GetCityResponseViewModel>>>>> 
        GetCities(
        [FromServices] ICityRepository citiesRepository,
        string? codigoIBGE, string? siglaEstado, string? nomeMunicipio,
        int skip = 0, int take = 250
        )
    {
        if ((take > 250 || take <= 0) || skip < 0) 
            return TypedResults.BadRequest(new ResultViewModel<string>("Parâmetros de busca inválidos. Valores aceitos: 'skip' mín.: 0 / 'take' mín.: 0 máx.: 250"));

        List<GetCityResponseViewModel> municipios = new();

        if (!String.IsNullOrEmpty(codigoIBGE))
        {
            var resultado = new List<Municipio> { await citiesRepository.GetByCodigoIBGE(codigoIBGE) }.Where(m => m != null).Select(x=> FormatCity(x)).ToList();
            return resultado.Any() ? TypedResults.Ok(new PagedResultViewModel<List<GetCityResponseViewModel>>(resultado, 0, 1, resultado.Count)) : TypedResults.NotFound();
        }
        else if (!String.IsNullOrEmpty(nomeMunicipio) && !String.IsNullOrEmpty(siglaEstado))
        {
            var resultado = (await citiesRepository.GetByNameAndState(nomeMunicipio, siglaEstado)).Where(m => m != null).Select(x => FormatCity(x)).ToList();
            return resultado.Any() ? TypedResults.Ok(new PagedResultViewModel<List<GetCityResponseViewModel>>(resultado, 0, 1, resultado.Count)) : TypedResults.NotFound();
        }
        else if (!String.IsNullOrEmpty(siglaEstado))
        {
            var totalCidadesDoEstado = await citiesRepository.CountCititesByState(siglaEstado);
            var resultado = (await citiesRepository.GetAllCitiesBySiglaUF(siglaEstado, skip, take)).Where(m => m != null).Select(x => FormatCity(x)).ToList();
            return resultado.Any() ? TypedResults.Ok(new PagedResultViewModel<List<GetCityResponseViewModel>>(resultado, skip, take, totalCidadesDoEstado)) : TypedResults.NotFound();
        }
        else if (!String.IsNullOrEmpty(nomeMunicipio))
        {
            var resultado = (await citiesRepository.GetByName(nomeMunicipio)).Where(m => m != null).Select(x => FormatCity(x)).ToList();
            return resultado.Any() ? TypedResults.Ok(new PagedResultViewModel<List<GetCityResponseViewModel>>(resultado, skip, take, resultado.Count)) : TypedResults.NotFound();
        }
        else
        {
            int citiesCount = await citiesRepository.CountCitites();
            var resultado = await citiesRepository.GetAllCities(skip, take);
            return resultado.Any() ? 
                TypedResults.Ok(new PagedResultViewModel<List<GetCityResponseViewModel>>(resultado.Select(x => FormatCity(x)).ToList(), skip, take, citiesCount)) :
                TypedResults.NotFound();
        }
    }

    public static async Task<Results<NotFound, BadRequest, Ok<ResultViewModel<Municipio>>>> UpdateCity (
        [FromServices] ICityRepository citiesRepository,
        string codigoIBGE,
        UpdateCityViewModel municipioAtualizado
        )
    {
        if (String.IsNullOrEmpty(municipioAtualizado.CodigoIBGE) &&
            String.IsNullOrEmpty(municipioAtualizado.NomeMunicipio))
            return TypedResults.BadRequest();

        var municipioASerAtualizado = await citiesRepository.GetByCodigoIBGE(codigoIBGE);
        if (municipioASerAtualizado is null) return TypedResults.NotFound();

        if (!String.IsNullOrEmpty(municipioAtualizado.NomeMunicipio))
            municipioASerAtualizado.NomeMunicipio = municipioAtualizado.NomeMunicipio.Trim();

        if (!String.IsNullOrEmpty(municipioAtualizado.CodigoIBGE))
            municipioASerAtualizado.CodigoIBGE = municipioAtualizado.CodigoIBGE.Trim();

        municipioASerAtualizado.ModificadoEm = DateTime.Now;

        var municipioFoiAtualizado = await citiesRepository.Update(municipioASerAtualizado);

        return TypedResults.Ok(new ResultViewModel<Municipio>(municipioASerAtualizado));
    }

    public static async Task<Results<NotFound, NoContent>> DeleteCity (
        [FromServices] ICityRepository citiesRepository,
        string codigoIBGE
        )
    {
        var municipioASerDeletado = await citiesRepository.GetByCodigoIBGE(codigoIBGE);
        if (municipioASerDeletado is null) return TypedResults.NotFound();

        var municipioFoiDeletado = await citiesRepository.Delete(municipioASerDeletado);

        return TypedResults.NoContent();
    }

    internal static GetCityResponseViewModel FormatCity(Municipio municipio) =>
        new GetCityResponseViewModel
        {
            CodigoIBGE = municipio.CodigoIBGE,
            NomeMunicipio = municipio.NomeMunicipio,
            NomeEstado = municipio.Estado.NomeUF,
            SiglaEstado = municipio.Estado.SiglaUF
        };
}