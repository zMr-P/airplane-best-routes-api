using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Interfaces;
using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Mapster;

namespace Airplane.Best.Routes.Application.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<Output<List<Route>>> GetAllRoutesAsync(CancellationToken cancellationToken)
        {
            var result = await _routeRepository.GetAllAsync(cancellationToken)
                .ConfigureAwait(false);

            if (result == null || result.Count == 0)
            {
                return new Output<List<Route>>(new(), false)
                    .AddErrorMessage("Nenhuma rota encontrada.");
            }

            return new Output<List<Route>>(result, true)
                .AddMessage("Sucesso ao recuperar as rotas.");
        }

        public async Task<Output<List<Route>>> GetRangeRoutesAsync(GetRoutesRequest request, CancellationToken cancellationToken)
        {
            var result = await _routeRepository.GetRangeAsync(request.Origin, request.Destination, cancellationToken)
                .ConfigureAwait(false);

            if (result == null || result.Count == 0)
            {
                return new Output<List<Route>>(new(), false)
                    .AddErrorMessage("Nenhuma rota encontrada.");
            }

            return new Output<List<Route>>(result, true)
                .AddMessage("Sucesso ao recuperar as rotas.");
        }

        public async Task<Output<Route>> GetBestRouteAsync(GetRoutesRequest request, CancellationToken cancellationToken)
        {
            var result = await _routeRepository.GetBestAsync(request.Origin, request.Destination, cancellationToken)
                .ConfigureAwait(false);

            if (result == null)
            {
                return new Output<Route>(new(), false)
                    .AddErrorMessage("Nenhuma rota encontrada.");
            }

            return new Output<Route>(result, true)
                .AddMessage("Sucesso ao recuperar a rota com melhor valor.");
        }

        public async Task<Output<Route>> CreateRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken)
        {
            var route = request.Adapt<Route>();

            var createdRoute = await _routeRepository.CreateAsync(route, cancellationToken)
                .ConfigureAwait(false);

            if (createdRoute == null)
            {
                return new Output<Route>(new(), false)
                    .AddErrorMessage("Erro ao criar a rota.");
            }

            return new Output<Route>(createdRoute, true)
                .AddMessage("Rota criada com sucesso.");
        }

        public async Task<Output<Route>> UpdateRouteAsync(Guid id, CreateRouteRequest request)
        {
            var routeToUpdate = request.Adapt<Route>();

            var updatedRoute = await _routeRepository.UpdateAsync(id, routeToUpdate)
                .ConfigureAwait(false);

            if (updatedRoute == null)
            {
                return new Output<Route>(new(), false)
                    .AddErrorMessage("Erro ao atualizar a rota. Verifique se a rota existe.");
            }

            return new Output<Route>(updatedRoute, true)
                .AddMessage("Rota atualizada com sucesso.");
        }

        public async Task<Output> DeleteRouteAsync(Guid id)
        {
            var isSuccess = await _routeRepository.DeleteAsync(id)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                return new Output(false)
                    .AddErrorMessage("Erro ao deletar a rota. Verifique se a rota existe.");
            }

            return new Output(true)
                .AddMessage("Rota deletada com sucesso.");
        }
    }
}
