using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Airplane.Best.Routes.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Airplane.Best.Routes.Infrastructure.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IMemoryContext _memoryContext;
        private readonly ILogger<RouteRepository> _logger;

        public RouteRepository(IMemoryContext memoryContext, ILogger<RouteRepository> logger)
        {
            _memoryContext = memoryContext;
            _logger = logger;
        }

        public async Task<List<Route>?> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _memoryContext.Routes
                    .Include(r => r.Connections)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionGetAllRoutes);
                return null;
            }
        }

        public async Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                return await _memoryContext.Routes
                    .Include(r => r.Connections)
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionGetRouteById);
                return null;
            }
        }

        public async Task<List<Route>?> GetRangeAsync(string originName, string destinationName, CancellationToken cancellationToken)
        {
            try
            {
                return await _memoryContext.Routes
                    .Where(r => r.OriginName == originName && r.DestinationName == destinationName)
                    .Include(r => r.Connections)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionGetRoutesByOriginAndDestination);
                return null;
            }
        }

        public async Task<Route?> GetBestAsync(string originName, string destinationName, CancellationToken cancellationToken)
        {
            try
            {
                return await _memoryContext.Routes
                    .Where(r => r.OriginName == originName && r.DestinationName == destinationName)
                    .OrderBy(r => r.Value)
                    .Include(r => r.Connections)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionGetBestRouteByOriginAndDestination);
                return null;
            }
        }

        public async Task<Route?> CreateAsync(Route route, CancellationToken cancellationToken)
        {
            try
            {
                var createdRoute = await _memoryContext.Routes.AddAsync(route, cancellationToken);
                await _memoryContext.SaveChangesAsync();

                return createdRoute.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionCreateRoute);
                return null;
            }
        }

        public async Task<Route?> UpdateAsync(Route newRoute)
        {
            try
            {
                var updatedRoute = _memoryContext.Routes.Update(newRoute);
                await _memoryContext.SaveChangesAsync();

                return updatedRoute.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionUpdateRoute);
                return null;
            }
        }

        public async Task<bool?> DeleteAsync(Route routeToDelete)
        {
            try
            {
                _memoryContext.Routes.Remove(routeToDelete);
                await _memoryContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.ExceptionDeleteRoute);
                return null;
            }
        }
    }
}