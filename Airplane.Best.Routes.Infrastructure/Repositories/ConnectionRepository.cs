using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Airplane.Best.Routes.Infrastructure.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly IMemoryContext _context;
        private readonly ILogger<ConnectionRepository> _logger;

        public ConnectionRepository(IMemoryContext context, ILogger<ConnectionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> ClearConnectionsByRouteId(Guid routeId, CancellationToken cancellation)
        {
            _logger.LogInformation($"Chamando connectionRepository");
            _logger.LogInformation($"Limpando conexões do routeId: {routeId}");

            try
            {
                var connections = await _context.Connections
                    .Where(c => c.RouteId == routeId)
                    .ToListAsync(cancellation)
                    .ConfigureAwait(false);

                _context.Connections.RemoveRange(connections);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao limpar conexões do routeId: {RouteId}", routeId);
                return false;
            }
        }
    }
}
