using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Application.ViewModels.Validators.State;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Localidades.Tests.ServiceTests;

public class StateServiceTests
{
    [Fact]
    public async Task CreateState_ReturnsConflict_WhenStateAlreadyExists()
    {
        // Arrange
        var mockRepo = new Mock<IStateRepository>();
        var inputState = new CreateStateViewModel
        {
            CodigoUF = "02",
            SiglaUF = "AB",
            NomeUF = "TestState"
        };

        mockRepo.Setup(repo => repo.AlreadyExists(It.IsAny<Estado>())).ReturnsAsync(true);

        // Act
        var result = await Application.Endpoints.StateEndpoints.CreateState(mockRepo.Object, inputState);

        // Assert
        Assert.IsType<Conflict<ResultViewModel<string>>>(result.Result);
    }

    [Fact]
    public async Task CreateState_ReturnsCreated_WhenStateIsNew()
    {
        var mockRepo = new Mock<IStateRepository>();
        var inputState = new CreateStateViewModel
        {
            CodigoUF = "12",
            SiglaUF = "AB",
            NomeUF = "TestState"
        };

        mockRepo.Setup(repo => repo.AlreadyExists(It.IsAny<Estado>())).ReturnsAsync(false);
        mockRepo.Setup(repo => repo.Create(It.IsAny<Estado>())).ReturnsAsync(true); // ou o retorno correto se for diferente

        var result = await Application.Endpoints.StateEndpoints.CreateState(mockRepo.Object, inputState);

        Assert.IsType<Created<ResultViewModel<Estado>>>(result.Result);
    }

    [Fact]
    public void CodigoUF_WithMoreThanTwoCharacters_IsInvalid()
    {
        var validator = new StateCreateRequestValidator();
        var model = new CreateStateViewModel
        {
            CodigoUF = "123",
            SiglaUF = "TS",
            NomeUF = "TestState"
        };

        var validationResult = validator.Validate(model);

        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "CodigoUF");
    }
}

