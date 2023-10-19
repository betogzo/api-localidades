using Localidades.Application.ViewModels.CityViewModels;
using FluentValidation;

namespace Localidades.Application.ViewModels.Validators.City;

public class CityUpdateRequestValidator : AbstractValidator<UpdateCityViewModel>
{

    public CityUpdateRequestValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.CodigoIBGE), () =>
        {
            RuleFor(x => x.CodigoIBGE)
                .Must(codigo => int.TryParse(codigo, out _)).WithMessage("O código IBGE é composto apenas por números.")
                .Length(7).WithMessage("O código IBGE deve conter 7 caracteres.");
        });
    }
}