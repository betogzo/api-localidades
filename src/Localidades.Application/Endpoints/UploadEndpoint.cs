using Localidades.Application.Configurations;
using Localidades.Application.ViewModels.ResultsViewModels;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Annotations;

namespace Localidades.Application.Endpoints;

public static class UploadEndpoint
{
    public static IEndpointRouteBuilder ConfigureUploadEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/upload/excel", LoadFromSpreadsheet)
               .WithMetadata(new SwaggerOperationAttribute
               {
                   Summary = "Extensões aceitas: .xls e .xlsx - Tamanho máx.: 2MB",
                   Description = @"Formato de arquivo aceito: planilha ESTADOS (headers: Codigo_UF, Sigla_UF, Nome_UF), planilha MUNICIPIOS (headers: Codigo_Municipio, Nome_Municipio e Codigo_UF).",
               })
               .RequireAuthorization(Roles.Admin);

        return app;
    }

    internal static async Task<Results<Ok, Conflict<ResultViewModel<string>>, BadRequest<ResultViewModel<string>>>> LoadFromSpreadsheet(
        [FromServices] IStateRepository statesRepository,
        [FromServices] ICityRepository citiesRepository,
        [FromForm] IFormFile planilha
        )
    {
        const long MAX_FILE_SIZE = 2L * 1024 * 1024;

        var extensao = Path.GetExtension(planilha.FileName).ToLowerInvariant();
        if (extensao != ".xls" && extensao != ".xlsx")
            return TypedResults.BadRequest(new ResultViewModel<string>("Extensão inválida de arquivo. Extensões aceitas: .xls ou .xlsx"));

        if (planilha.Length > MAX_FILE_SIZE)
            return TypedResults.BadRequest(new ResultViewModel<string>("O arquivo excede o tamanho máximo permitido (2MB)."));

        try
        {
            using var package = new ExcelPackage(planilha.OpenReadStream());

            await ReadAndInsertNewStates(package, statesRepository);
            await ReadAndInsertNewCities(package, citiesRepository);
        }
        catch (Exception ex)
        {
            return TypedResults.Conflict(new ResultViewModel<string>($"Erro ao inserir dados no banco a partir do arquivo. Motivo: {ex.Message}"));
        }

        return TypedResults.Ok();
    }

    internal async static Task<bool> ReadAndInsertNewStates(ExcelPackage package, IStateRepository statesRepository)
    {
        var estadosBanco = await statesRepository.GetAllEstados();

        var estadosPlanilhaExcel = package.Workbook.Worksheets["ESTADOS"];

        if (estadosPlanilhaExcel is null) 
            throw new Exception("Planilha 'ESTADOS' não encontrada. Forneça um arquivo válido.");

        var estadosLista = estadosPlanilhaExcel.Cells["A2:C" + estadosPlanilhaExcel.Dimension.End.Row]
                          .GroupBy(cell => cell.Start.Row)
                          .Select(g => new Estado
                          {
                              CodigoUF = g.First(cell => cell.Start.Column == 1).Text,
                              SiglaUF = g.First(cell => cell.Start.Column == 2).Text,
                              NomeUF = g.First(cell => cell.Start.Column == 3).Text
                          }).ToList();

        var estadosASeremGravados = estadosLista
            .Where(excelState => !estadosBanco.Any(dbState => dbState.CodigoUF == excelState.CodigoUF))
            .ToList();


        foreach (var estado in estadosASeremGravados)
        {
            await statesRepository.Create(estado);
        }

        return true;
    }

    internal async static Task<bool> ReadAndInsertNewCities(ExcelPackage package, ICityRepository citiesRepository)
    {
        var cidadesBanco = await citiesRepository.GetAllCities();

        var municipioPlanilhaExcel = package.Workbook.Worksheets["MUNICIPIOS"];

        if (municipioPlanilhaExcel is null)
            throw new Exception("Planilha 'MUNICIPIOS' não encontrada. Forneça um arquivo válido.");

        var municipiosLista = municipioPlanilhaExcel.Cells["A2:C" + municipioPlanilhaExcel.Dimension.End.Row]
                            .GroupBy(cell => cell.Start.Row)
                            .Select(g => new Municipio
                            {
                                CodigoIBGE = g.First(cell => cell.Start.Column == 1).Text,
                                NomeMunicipio = g.First(cell => cell.Start.Column == 2).Text,
                                CodigoUF = g.First(cell => cell.Start.Column == 3).Text
                            }).ToList();

        var municipiosASeremGravados = municipiosLista
            .Where(excelCity => !cidadesBanco.Any(dbCity => dbCity.CodigoIBGE == excelCity.CodigoIBGE))
            .ToList();

        await citiesRepository.BulkInsertion(municipiosASeremGravados);

        return true;
    }
}