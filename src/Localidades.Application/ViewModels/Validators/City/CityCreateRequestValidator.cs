using Localidades.Application.ViewModels.CityViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Localidades.Application.ViewModels.Validators.City;

public class CityCreateRequestValidator : AbstractValidator<CreateCityViewModel>
{
    public CityCreateRequestValidator()
    {
        RuleFor(x => x.CodigoUF)
            .NotEmpty().WithMessage("É necessário informar o código do Estado ao qual o município pertence.")
            .Must(codigo => int.TryParse(codigo, out _)).WithMessage("O código do Estado é composto apenas por números.")
            .Length(2).WithMessage("O código do Estado ao qual o município pertence deve conter 2 caracteres.");

        RuleFor(x => x.CodigoIBGE)
            .NotEmpty().WithMessage("É necessário informar o código IBGE do município.")
            .Must(codigo => int.TryParse(codigo, out _)).WithMessage("O código IBGE é composto apenas por números.")
            .Length(7).WithMessage("O código IBGE deve conter 7 caracteres.");

        RuleFor(x => x.NomeMunicipio)
            .NotEmpty().WithMessage("É necessário informar o nome do município.")
            .Must(value => Regex.IsMatch(value, @"^[a-zA-ZáÁàÀãÃâÂéÉêÊíÍóÓõÕôÔúÚçÇ\s\-']*?$")).WithMessage("O nome do município não pode conter números ou caracteres especiais.")
            .MinimumLength(4).WithMessage("O nome do município deve conter no mínimo 3 caracteres.")
            .MaximumLength(80).WithMessage("O nome do município deve conter no máximo 50 caracteres.");
    }
}