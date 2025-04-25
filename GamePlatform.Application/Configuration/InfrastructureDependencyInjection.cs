using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GamePlatform.Infrastructure.Contexts;
using GamePlatform.Domain.Interfaces;
using GamePlatform.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace GamePlatform.Application.Configuration;

public static class InfrastructureDependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
    }
}