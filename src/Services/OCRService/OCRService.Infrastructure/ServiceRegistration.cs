using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OCRService.Domain.Interfaces;
using OCRService.Infrastructure.Persistence;
using OCRService.Infrastructure.Repositories;

namespace OCRService.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OCRDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IOcrResultRepository, OcrResultRepository>();

        return services;
    }
}
