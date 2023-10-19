namespace Localidades.Application.Configurations;

public static class ConfigureSwaggerUI
{
    public static void ConfigureCustomSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(c =>
        {
            c.OAuthUsePkce();
        });
    }
}