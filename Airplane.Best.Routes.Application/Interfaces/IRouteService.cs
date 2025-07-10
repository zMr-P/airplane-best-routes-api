using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Domain.Entities;

namespace Airplane.Best.Routes.Application.Interfaces
{
    public interface IRouteService
    {
        Task<Output<List<Route>>> GetAllRoutesAsync(CancellationToken cancellationToken);
        Task<Output<List<Route>>> GetRangeRoutesAsync(GetRoutesRequest request, CancellationToken cancellationToken);
        Task<Output<Route>> GetBestRouteAsync(GetRoutesRequest request, CancellationToken cancellationToken);
        Task<Output<Route>> CreateRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken);
        Task<Output<Route>> UpdateRouteAsync(Guid id, CreateRouteRequest request);
        Task<Output> DeleteRouteAsync(Guid id);
    }
}
