using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Airplane.Best.Routes.Domain.Interfaces.Services;
using Airplane.Best.Routes.Domain.Models;
using Mapster;

namespace Airplane.Best.Routes.Application.Services
{
    public class BestRouteService : IBestRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IConnectionRepository _connectionRepository;

        public BestRouteService(IRouteRepository routeRepository, IConnectionRepository connectionRepository)
        {
            _routeRepository = routeRepository;
            _connectionRepository = connectionRepository;
        }

        public async Task<List<Route>?> GetAllRoutesAsync(CancellationToken cancellationToken)
        {
            return await _routeRepository.GetAllAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Route?> GetBestRouteAsync(GetRoutesModel request, CancellationToken cancellationToken)
        {
            return await _routeRepository.GetBestAsync(request.Origin, request.Destination, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Route>?> GetRangeRoutesAsync(GetRoutesModel request, CancellationToken cancellationToken)
        {
            return await _routeRepository.GetRangeAsync(request.Origin, request.Destination, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Route?> CreateRouteAsync(CreateRouteModel request, CancellationToken cancellationToken)
        {
            var route = request.Adapt<Route>();
            route.Id = Guid.NewGuid();

            if (route.Connections != null && route.Connections.Any() && route.Connections.Count > 0)
            {
                foreach (var connection in route.Connections)
                {
                    connection.Id = Guid.NewGuid();
                    connection.RouteId = route.Id;
                }
            }

            var createdRoute = await _routeRepository.CreateAsync(route, cancellationToken)
                .ConfigureAwait(false);

            return createdRoute;
        }

        public async Task<Route?> UpdateRouteAsync(UpdateRouteModel request, CancellationToken cancellationToken)
        {
            var dataRoute = await _routeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (dataRoute == null)
                return null;

            var isDeleted = await _connectionRepository.ClearConnectionsByRouteId(dataRoute.Id, cancellationToken);
            if (!isDeleted)
                return null;

            dataRoute.OriginName = request.OriginName;
            dataRoute.DestinationName = request.DestinationName;
            dataRoute.IsAvaiable = request.IsAvaiable;
            dataRoute.Value = request.Value;

            if (request.Connections != null && request.Connections.Count > 0)
            {
                dataRoute.Connections = request.Connections.Select(c => c.Adapt<Connection>()).ToList();
                foreach (var connection in dataRoute.Connections)
                {
                    connection.Id = Guid.NewGuid();
                    connection.RouteId = dataRoute.Id;
                }
            }
            else
            {
                dataRoute.Connections.Clear();
            }

            var updatedRoute = await _routeRepository.UpdateAsync(dataRoute)
                .ConfigureAwait(false);

            return updatedRoute;
        }

        public async Task<bool> DeleteRouteAsync(Guid id, CancellationToken cancellationToken)
        {
            var routeToDelete = await _routeRepository.GetByIdAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (routeToDelete == null)
                return false;

            var isDeleted = await _routeRepository.DeleteAsync(routeToDelete);
            if (isDeleted == null || isDeleted == false)
                return false;

            return true;
        }
    }
}
