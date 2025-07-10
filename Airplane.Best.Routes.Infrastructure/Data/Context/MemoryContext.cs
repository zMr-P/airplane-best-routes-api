using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Microsoft.EntityFrameworkCore;

namespace Airplane.Best.Routes.Infrastructure.Data.Context
{
    public class MemoryContext : DbContext, IMemoryContext
    {
        public MemoryContext(DbContextOptions<MemoryContext> options) : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>()
                .HasMany(r => r.Connections)
                .WithOne(c => c.Route)
                .HasForeignKey(c => c.RouteId);

            var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            var jsonRoutesFilePath = Path.Combine(solutionDirectory, "airplane-best-routes-api", "Airplane.Best.Routes.Infrastructure", "Data", "Created", "routes-seed.json");
            if (File.Exists(jsonRoutesFilePath))
            {
                var jsonRoutes = File.ReadAllText(jsonRoutesFilePath);
                var routes = System.Text.Json.JsonSerializer.Deserialize<List<Route>>(jsonRoutes);

                if (routes != null)
                {
                    foreach (var route in routes)
                    {
                        var routeSeed = new Route
                        {
                            Id = route.Id,
                            OriginName = route.OriginName,
                            DestinationName = route.DestinationName,
                            IsAvaiable = route.IsAvaiable,
                            Value = route.Value
                        };
                        modelBuilder.Entity<Route>().HasData(routeSeed);
                    }
                }
            }

            var jsonConnectionsFilePath = Path.Combine(solutionDirectory, "airplane-best-routes-api", "Airplane.Best.Routes.Infrastructure", "Data", "Created", "connections-seed.json");
            if (File.Exists(jsonConnectionsFilePath))
            {
                var jsonConnections = File.ReadAllText(jsonConnectionsFilePath);
                var connections = System.Text.Json.JsonSerializer.Deserialize<List<Connection>>(jsonConnections);

                if (connections != null)
                {
                    foreach (var connection in connections)
                    {
                        var connectionSeed = new Connection
                        {
                            Id = connection.Id,
                            RouteId = connection.RouteId,
                            Name = connection.Name
                        };
                        modelBuilder.Entity<Connection>().HasData(connectionSeed);
                    }
                }
            }
        }
    }
}
