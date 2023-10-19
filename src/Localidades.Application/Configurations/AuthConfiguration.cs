using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Localidades.Application.Configurations;

public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuthenticationAuthorization(this IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes(Settings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
     .AddJwtBearer(x =>
     {
         x.RequireHttpsMetadata = true;
         x.SaveToken = true;
         x.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(key),
             ValidateIssuer = false,
             ValidateAudience = false
         };
     });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
        });

        return services;
    }
}

