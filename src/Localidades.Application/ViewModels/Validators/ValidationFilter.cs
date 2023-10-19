using Localidades.Application.ViewModels.ResultsViewModels;
using FluentValidation;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is not null)
        {
            var modelName = context.Arguments
              .OfType<T>()
              .FirstOrDefault(a => a?.GetType() == typeof(T));

            if (modelName is not null)
            {
                var validation = await validator.ValidateAsync(modelName);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();

                    var viewModel = new ResultViewModel<string>(errors);

                    return viewModel;
                }

                return await next(context);
            }
            else
            {
                var viewModel = new ResultViewModel<string>("Não foi possível identificar a entidade a ser validada.");
                return viewModel;
            }
        }

        return await next(context);
    }
}