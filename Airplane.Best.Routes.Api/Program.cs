using Airplane.Best.Routes.Application.Mapper;
using Airplane.Best.Routes.Infrastructure.Ioc.Builder;
using Airplane.Best.Routes.Infrastructure.Ioc.Extensions;

namespace airplane_best_routes_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.SetSwaggerConfig();
            builder.Services.SetVersioningConfig();
            builder.Services.SetContextConfig();
            builder.Services.SetRepositoriesConfig();
            builder.Services.SetServicesConfig();
            MapsterProfile.RegisterMappings();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.EnsureDatabaseCreated();

            app.Run();
        }
    }
}
