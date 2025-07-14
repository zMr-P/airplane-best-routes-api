using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Dtos.RouteService.Response;
using Airplane.Best.Routes.Domain.Entities;

namespace Airplane.Best.Routes.Application.Interfaces
{
    public interface IBestRoutesApplication
    {
        Task<Output<List<GetRouteResponseDto>>> GetAllRoutesAsync(CancellationToken cancellationToken);
        Task<Output<List<GetRouteResponseDto>>> GetRangeRoutesAsync(GetRoutesRequestDto request, CancellationToken cancellationToken);
        Task<Output<GetRouteResponseDto>> GetBestRouteAsync(GetRoutesRequestDto request, CancellationToken cancellationToken);
        Task<Output<GetRouteResponseDto>> CreateRouteAsync(CreateRouteRequestDto request, CancellationToken cancellationToken);
        Task<Output<GetRouteResponseDto>> UpdateRouteAsync(Guid id, UpdateRouteRequestDto request, CancellationToken cancellationToken);
        Task<Output> DeleteRouteAsync(Guid id, CancellationToken cancellationToken);
    }
}
