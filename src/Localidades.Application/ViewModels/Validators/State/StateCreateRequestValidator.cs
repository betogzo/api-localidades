using Localidades.Application.ViewModels.StateViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Localidades.Application.ViewModels.Validators.State;

public class StateCreateRequestValidator : AbstractValidator<CreateStateViewModel>
{
    public StateCreateRequestValidator()
    {
        RuleFor(x => x.CodigoUF)
            .NotEmpty().WithMessage("É necessário informar o código do Estado.")
            .Must(codigo => int.TryParse(codigo, out _)).WithMessage("O código do Estado é composto apenas por números.")
            .Length(2).WithMessage("O código do Estado deve conter 2 caracteres.");

        RuleFor(x => x.SiglaUF)
            .NotEmpty().WithMessage("É necessário informar a sigla do Estado.")
            .Must(value => Regex.IsMatch(value, @"^[a-zA-Z\s]*$")).WithMessage("A sigla do Estado não pode conter números ou caracteres especiais.")
            .Length(2).WithMessage("A sigla do Estado deve conter 2 caracteres.");

        RuleFor(x => x.NomeUF)
            .NotEmpty().WithMessage("É necessário informar o nome do Estado.")
            .Must(value => Regex.IsMatch(value, @"^[a-zA-ZáÁàÀãÃâÂéÉêÊíÍóÓõÕôÔúÚçÇ\s]*$")).WithMessage("O nome do Estado não pode conter números ou caracteres especiais.")
            .MinimumLength(4).WithMessage("O nome do Estado deve conter no mínimo 4 caracteres.")
            .MaximumLength(50).WithMessage("O nome do Estado deve conter no máximo 50 caracteres.");
    }
}