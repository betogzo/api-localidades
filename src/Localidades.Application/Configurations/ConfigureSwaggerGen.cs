using Microsoft.OpenApi.Models;

namespace Localidades.Application.Configurations;

public static class SwaggerGen
{
    public static IServiceCollection ConfigureCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
         {
             c.SwaggerDoc("v1", new OpenApiInfo
             {
                 Version = "V1",
                 Title = "API Localidades",
                 Description = "Cadastro e pesquisa de municípios e estados do Brasil.",
                 Contact = new OpenApiContact
                 {
                     Name = "Repositório GitHub",
                     Url = new Uri("https://github.com/betogzo/api-localidades"),
                 }
             });
             c.EnableAnnotations();
             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Name = "Authorization",
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer",
                 BearerFormat = "JWT",
                 In = ParameterLocation.Header,
                 Description = "Formato a ser digitado abaixo: 'Bearer <token>"
             });
             c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
         });


        return services;
    }
}

