using Localidades.Application.Endpoints;
using Localidades.Application.ViewModels.CityViewModels;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Localidades.Tests.ServiceTests;

public class CityEndpointsTests
{
    [Fact]
    public async Task CreateCity_ReturnsConflict_WhenStateDoesNotExist()
    {
        var cities = new Mock<ICityRepository>();
        var states = new Mock<IStateRepository>();
        states.Setup(s => s.GetByCodigoUF("99"))!.ReturnsAsync((Estado?)null);
        var body = new CreateCityViewModel
        {
            CodigoIBGE = "1234567",
            CodigoUF = "99",
            NomeMunicipio = "X"
        };

        var result = await CityEndpoints.CreateCity(cities.Object, states.Object, body);

        Assert.IsType<Conflict<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task CreateCity_ReturnsConflict_WhenCityAlreadyExists()
    {
        var cities = new Mock<ICityRepository>();
        cities.Setup(c => c.AlreadyExists(It.IsAny<Municipio>())).ReturnsAsync(true);
        var states = new Mock<IStateRepository>();
        states.Setup(s => s.GetByCodigoUF("35"))
            .ReturnsAsync(new Estado { CodigoUF = "35", SiglaUF = "SP", NomeUF = "SP" });
        var body = new CreateCityViewModel
        {
            CodigoIBGE = "3550308",
            CodigoUF = "35",
            NomeMunicipio = "São Paulo"
        };

        var result = await CityEndpoints.CreateCity(cities.Object, states.Object, body);

        Assert.IsType<Conflict<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task CreateCity_ReturnsCreated_WhenValid()
    {
        var cities = new Mock<ICityRepository>();
        cities.Setup(c => c.AlreadyExists(It.IsAny<Municipio>())).ReturnsAsync(false);
        cities.Setup(c => c.Create(It.IsAny<Municipio>())).ReturnsAsync(true);
        var states = new Mock<IStateRepository>();
        states.Setup(s => s.GetByCodigoUF("35"))
            .ReturnsAsync(new Estado { CodigoUF = "35", SiglaUF = "SP", NomeUF = "SP" });
        var body = new CreateCityViewModel
        {
            CodigoIBGE = "3550308",
            CodigoUF = "35",
            NomeMunicipio = "São Paulo"
        };

        var result = await CityEndpoints.CreateCity(cities.Object, states.Object, body);

        Assert.IsType<Created<ResultViewModel<Municipio>>>(result.Result);
    }

    [Fact]
    public async Task GetCities_ReturnsBadRequest_WhenTakeAboveLimit()
    {
        var repo = new Mock<ICityRepository>();

        var result = await CityEndpoints.GetCities(repo.Object, null, null, null, 0, 251);

        Assert.IsType<BadRequest<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task GetCities_ReturnsBadRequest_WhenTakeIsZero()
    {
        var repo = new Mock<ICityRepository>();

        var result = await CityEndpoints.GetCities(repo.Object, null, null, null, 0, 0);

        Assert.IsType<BadRequest<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task GetCities_ReturnsBadRequest_WhenSkipIsNegative()
    {
        var repo = new Mock<ICityRepository>();

        var result = await CityEndpoints.GetCities(repo.Object, null, null, null, -1, 10);

        Assert.IsType<BadRequest<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task GetCities_ReturnsNotFound_WhenListEmpty()
    {
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.CountCitites()).ReturnsAsync(0);
        repo.Setup(r => r.GetAllCities(0, 250)).ReturnsAsync(new List<Municipio>());

        var result = await CityEndpoints.GetCities(repo.Object, null, null, null, 0, 250);

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task GetCities_ReturnsOk_WhenAllCitiesReturned()
    {
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.CountCitites()).ReturnsAsync(1);
        var municipio = new Municipio
        {
            CodigoIBGE = "3550308",
            NomeMunicipio = "São Paulo",
            Estado = new Estado { NomeUF = "São Paulo", SiglaUF = "SP" }
        };
        repo.Setup(r => r.GetAllCities(0, 250)).ReturnsAsync(new List<Municipio> { municipio });

        var result = await CityEndpoints.GetCities(repo.Object, null, null, null, 0, 250);

        Assert.IsType<Ok<PagedResultViewModel<List<GetCityResponseViewModel>>>>(result.Result);
    }

    [Fact]
    public async Task UpdateCity_ReturnsBadRequest_WhenNoUpdatableFields()
    {
        var repo = new Mock<ICityRepository>();
        var body = new UpdateCityViewModel { CodigoIBGE = null, NomeMunicipio = null };

        var result = await CityEndpoints.UpdateCity(repo.Object, "3550308", body);

        Assert.IsType<BadRequest>(result.Result);
    }

    [Fact]
    public async Task UpdateCity_ReturnsNotFound_WhenCityMissing()
    {
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.GetByCodigoIBGE("999"))!.ReturnsAsync((Municipio?)null);
        var body = new UpdateCityViewModel { NomeMunicipio = "Novo" };

        var result = await CityEndpoints.UpdateCity(repo.Object, "999", body);

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task UpdateCity_ReturnsOk_WhenCityUpdated()
    {
        var existente = new Municipio
        {
            CodigoIBGE = "3550308",
            NomeMunicipio = "São Paulo",
            Estado = new Estado { SiglaUF = "SP", NomeUF = "SP" }
        };
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.GetByCodigoIBGE("3550308")).ReturnsAsync(existente);
        repo.Setup(r => r.Update(It.IsAny<Municipio>())).ReturnsAsync(true);
        var body = new UpdateCityViewModel { NomeMunicipio = "São Paulo (capital)" };

        var result = await CityEndpoints.UpdateCity(repo.Object, "3550308", body);

        var ok = Assert.IsType<Ok<ResultViewModel<Municipio>>>(result.Result);
        Assert.Equal("São Paulo (capital)", ok.Value?.Data?.NomeMunicipio);
    }

    [Fact]
    public async Task DeleteCity_ReturnsNotFound_WhenCityMissing()
    {
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.GetByCodigoIBGE("999"))!.ReturnsAsync((Municipio?)null);

        var result = await CityEndpoints.DeleteCity(repo.Object, "999");

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteCity_ReturnsNoContent_WhenDeleted()
    {
        var m = new Municipio { CodigoIBGE = "3550308", NomeMunicipio = "SP" };
        var repo = new Mock<ICityRepository>();
        repo.Setup(r => r.GetByCodigoIBGE("3550308")).ReturnsAsync(m);
        repo.Setup(r => r.Delete(m)).ReturnsAsync(true);

        var result = await CityEndpoints.DeleteCity(repo.Object, "3550308");

        Assert.IsType<NoContent>(result.Result);
    }
}
