using Localidades.Application.Endpoints;
using Localidades.Domain.Interfaces.Repositories;
using Localidades.Infrastructure.Data;
using Localidades.Infrastructure.Repositories;
using Localidades.Infrastructure.Repositories.Implementations;

namespace Localidades.Application.Services;

public static class ServiceExtensions
{
    public static void ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<PasswordHasherService>();
        builder.Services.AddSingleton<TokenService>();

        builder.Services.AddScoped<DataContext>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IStateRepository, StatesRepository>();
        builder.Services.AddScoped<ICityRepository, CitiesRepository>();
    }
}