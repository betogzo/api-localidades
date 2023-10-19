using Localidades.Application.Configurations;
using Localidades.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Localidades.Application.Services;

public static class DbContextExtensions
{
    public static void AddSqlServerDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Settings.ConnectionString));
    }
}