using Localidades.Application.ViewModels.ResultsViewModels;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Localidades.Application.Configurations;

public static class GlobalErrorHandling
{
    public static IApplicationBuilder ConfigureCustomGlobalErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var exception = context.Features.Get<IExceptionHandlerFeature>();
                if (exception != null)
                {
                    var errorResponse = new ResultViewModel<string>($"Ocorreu um erro interno no serviço. Motivo: {exception.Error.InnerException}");

                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });
        });
        return app;
    }
}