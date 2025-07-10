using Airplane.Best.Routes.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Airplane.Best.Routes.Infrastructure.Ioc.Builder
{
    public static class ApplicationBuilder
    {
        public static void EnsureDatabaseCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetService<MemoryContext>();
            context.Database.EnsureCreated();
        }
    }
}
