using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Models;

namespace Airplane.Best.Routes.Domain.Interfaces.Services
{
    public interface IBestRouteService
    {
        Task<List<Route>?> GetAllRoutesAsync(CancellationToken cancellationToken);
        Task<List<Route>?> GetRangeRoutesAsync(GetRoutesModel request, CancellationToken cancellationToken);
        Task<Route?> GetBestRouteAsync(GetRoutesModel request, CancellationToken cancellationToken);
        Task<Route?> CreateRouteAsync(CreateRouteModel request, CancellationToken cancellationToken);
        Task<Route?> UpdateRouteAsync(UpdateRouteModel request, CancellationToken cancellationToken);
        Task<bool> DeleteRouteAsync(Guid id, CancellationToken cancellationToken);
    }
}
