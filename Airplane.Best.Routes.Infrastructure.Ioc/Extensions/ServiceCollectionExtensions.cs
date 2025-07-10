using Airplane.Best.Routes.Application.Interfaces;
using Airplane.Best.Routes.Application.Services;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Airplane.Best.Routes.Infrastructure.Data.Context;
using Airplane.Best.Routes.Infrastructure.Repositories;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Airplane.Best.Routes.Infrastructure.Ioc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetContextConfig(this IServiceCollection services)
        {
            services.AddDbContext<MemoryContext>(options =>
                  options.UseInMemoryDatabase("AirplaneBestRoutesDb"));

            services.AddScoped<IMemoryContext, MemoryContext>();
            return services;
        }

        public static IServiceCollection SetRepositoriesConfig(this IServiceCollection services)
        {
            services.AddScoped<IRouteRepository, RouteRepository>();
            return services;
        }

        public static IServiceCollection SetServicesConfig(this IServiceCollection services)
        {
            services.AddScoped<IRouteService, RouteService>();
            return services;
        }

        public static IServiceCollection SetVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }

        public static IServiceCollection SetSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Airplane Best Routes API",
                    Version = "v1",
                    Description = "API for managing and retrieving the best routes for airplanes."
                });
            });
            return services;
        }
    }
}
