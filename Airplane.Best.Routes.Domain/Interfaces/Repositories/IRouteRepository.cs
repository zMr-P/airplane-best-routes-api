using Airplane.Best.Routes.Domain.Entities;

namespace Airplane.Best.Routes.Domain.Interfaces.Repositories
{
    public interface IRouteRepository
    {
        Task<List<Route>?> GetAllAsync(CancellationToken cancellationToken);
        Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Route>?> GetRangeAsync(string originName, string destinationName, CancellationToken cancellationToken);
        Task<Route?> GetBestAsync(string originName, string destinationName, CancellationToken cancellationToken);
        Task<Route?> CreateAsync(Route route, CancellationToken cancellationToken);
        Task<Route?> UpdateAsync(Route newRoute);
        Task<bool?> DeleteAsync(Route routeToDelete);
    }
}
