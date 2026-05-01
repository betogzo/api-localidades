using Localidades.Application.Endpoints;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Localidades.Tests.ServiceTests;

public class StateEndpointsExtendedTests
{
    [Fact]
    public async Task GetStates_ReturnsOk_WhenRepositoryReturnsList()
    {
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetAllEstados()).ReturnsAsync(new List<Estado>
        {
            new() { CodigoUF = "35", SiglaUF = "SP", NomeUF = "São Paulo" }
        });

        var result = await StateEndpoints.GetStates(repo.Object, null, null, null);

        Assert.IsType<Ok<ResultViewModel<List<Estado>>>>(result.Result);
    }

    [Fact]
    public async Task GetStates_ReturnsNotFound_WhenNoMatchForCode()
    {
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("99")).Returns(Task.FromResult<Estado>(null!));

        var result = await StateEndpoints.GetStates(repo.Object, "99", null, null);

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task GetStates_ReturnsOk_WhenFoundByCode()
    {
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("35")).ReturnsAsync(new Estado
        {
            CodigoUF = "35",
            SiglaUF = "SP",
            NomeUF = "São Paulo"
        });

        var result = await StateEndpoints.GetStates(repo.Object, "35", null, null);

        Assert.IsType<Ok<ResultViewModel<List<Estado>>>>(result.Result);
    }

    [Fact]
    public async Task UpdateState_ReturnsNotFound_WhenStateMissing()
    {
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("00")).Returns(Task.FromResult<Estado>(null!));
        var body = new UpdateStateViewModel { NomeUF = "X" };

        var result = await StateEndpoints.UpdateState(repo.Object, "00", body);

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task UpdateState_ReturnsOk_WhenStateExists()
    {
        var existing = new Estado { CodigoUF = "35", SiglaUF = "SP", NomeUF = "São Paulo" };
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("35")).ReturnsAsync(existing);
        repo.Setup(r => r.Update(It.IsAny<Estado>())).ReturnsAsync(true);
        var body = new UpdateStateViewModel { NomeUF = "São Paulo Atualizado" };

        var result = await StateEndpoints.UpdateState(repo.Object, "35", body);

        var ok = Assert.IsType<Ok<ResultViewModel<Estado>>>(result.Result);
        Assert.Equal("São Paulo Atualizado", ok.Value?.Data?.NomeUF);
    }

    [Fact]
    public async Task DeleteState_ReturnsNotFound_WhenStateMissing()
    {
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("00")).Returns(Task.FromResult<Estado>(null!));

        var result = await StateEndpoints.DeleteState(repo.Object, "00");

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteState_ReturnsNoContent_WhenDeleted()
    {
        var estado = new Estado { CodigoUF = "35", SiglaUF = "SP", NomeUF = "SP" };
        var repo = new Mock<IStateRepository>();
        repo.Setup(r => r.GetByCodigoUF("35")).ReturnsAsync(estado);
        repo.Setup(r => r.Delete(estado)).ReturnsAsync(true);

        var result = await StateEndpoints.DeleteState(repo.Object, "35");

        Assert.IsType<NoContent>(result.Result);
    }
}
