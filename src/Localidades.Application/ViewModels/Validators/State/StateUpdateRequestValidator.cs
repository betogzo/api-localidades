using Localidades.Application.ViewModels.StateViewModels;
using Localidades.Domain.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Localidades.Application.ViewModels.Validators.State;

public class StateEndpointRequestValidator : AbstractValidator<UpdateStateViewModel>
{
    public StateEndpointRequestValidator()
    {
        RuleFor(x => x.SiglaUF)
            .Must(value => Regex.IsMatch(value, @"^[a-zA-Z\s]*$")).WithMessage("A sigla do Estado não pode conter números ou caracteres especiais.")
            .Length(2).WithMessage("A sigla do Estado deve conter 2 caracteres.");

        RuleFor(x => x.NomeUF)
            .MinimumLength(4).WithMessage("O nome do Estado deve conter no mínimo 50 caracteres.")
            .Must(value => Regex.IsMatch(value, @"^[a-zA-ZáÁàÀãÃâÂéÉêÊíÍóÓõÕôÔúÚçÇ\s]*$")).WithMessage("O nome do Estado não pode conter números ou caracteres especiais.")
            .MaximumLength(50).WithMessage("O nome do Estado deve conter no máximo 50 caracteres.");
    }
}