using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Airplane.Best.Routes.Infrastructure.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IMemoryContext _memoryContext;

        public RouteRepository(IMemoryContext memoryContext)
        {
            _memoryContext = memoryContext;
        }

        public async Task<List<Route>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _memoryContext.Routes
                .Include(r => r.Connections)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Route>> GetRangeAsync(string originName, string destinationName, CancellationToken cancellationToken)
        {
            return await _memoryContext.Routes
                .Where(r => r.OriginName == originName && r.DestinationName == destinationName)
                .Include(r => r.Connections)
                .ToListAsync(cancellationToken);
        }

        public async Task<Route?> GetBestAsync(string originName, string destinationName, CancellationToken cancellationToken)
        {
            return await _memoryContext.Routes
                .Where(r => r.OriginName == originName && r.DestinationName == destinationName)
                .OrderBy(r => r.Value)
                .Include(r => r.Connections)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Route> CreateAsync(Route route, CancellationToken cancellationToken)
        {
            route.Id = Guid.NewGuid();
            foreach (var connection in route.Connections)
            {
                connection.Id = Guid.NewGuid();
                connection.RouteId = route.Id;
            }

            var createdRoute = await _memoryContext.Routes.AddAsync(route, cancellationToken);
            await _memoryContext.SaveChangesAsync();

            return createdRoute.Entity;
        }

        public async Task<Route?> UpdateAsync(Guid id, Route newRoute)
        {
            var dataRoute = await _memoryContext.Routes
                .Include(r => r.Connections)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (dataRoute == null)
                return null;

            dataRoute.OriginName = newRoute.OriginName;
            dataRoute.DestinationName = newRoute.DestinationName;
            dataRoute.Value = newRoute.Value;

            if (dataRoute.Connections != null && dataRoute.Connections.Any())
            {
                foreach (var conn in dataRoute.Connections.ToList())
                {
                    _memoryContext.Connections.Remove(conn);
                }
            }

            dataRoute.Connections = new List<Connection>();

            foreach (var connection in newRoute.Connections)
            {
                var newConnection = new Connection
                {
                    Id = Guid.NewGuid(),
                    RouteId = dataRoute.Id,
                    Name = connection.Name
                };
                dataRoute.Connections.Add(newConnection);
                _memoryContext.Connections.Add(newConnection);
            }

            await _memoryContext.SaveChangesAsync();

            return dataRoute;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var route = await _memoryContext.Routes.FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
                return false;

            _memoryContext.Routes.Remove(route);
            await _memoryContext.SaveChangesAsync();

            return true;
        }
    }
}
