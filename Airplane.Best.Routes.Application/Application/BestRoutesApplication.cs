using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Dtos.RouteService.Response;
using Airplane.Best.Routes.Application.Interfaces;
using Airplane.Best.Routes.Domain.Interfaces.Services;
using Airplane.Best.Routes.Domain.Messages;
using Airplane.Best.Routes.Domain.Models;
using Mapster;

namespace Airplane.Best.Routes.Application.Application
{
    public class BestRoutesApplication : IBestRoutesApplication
    {
        private readonly IBestRouteService _bestRouteService;

        public BestRoutesApplication(IBestRouteService bestRouteService)
        {
            _bestRouteService = bestRouteService;
        }

        public async Task<Output<List<GetRouteResponseDto>>> GetAllRoutesAsync(CancellationToken cancellationToken)
        {
            var result = await _bestRouteService.GetAllRoutesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (result == null || result.Count == 0)
            {
                return new Output<List<GetRouteResponseDto>>(new(), false)
                    .AddErrorMessage(ErrorMessages.NoRoutesFound);
            }

            var responseList = new List<GetRouteResponseDto>();
            foreach (var route in result)
            {
                var response = route.Adapt<GetRouteResponseDto>();
                responseList.Add(response);
            }

            return new Output<List<GetRouteResponseDto>>(responseList, true)
                .AddMessage(SuccessMessages.RoutesSearch);
        }

        public async Task<Output<List<GetRouteResponseDto>>> GetRangeRoutesAsync(GetRoutesRequestDto request, CancellationToken cancellationToken)
        {
            var getRoutesModel = request.Adapt<GetRoutesModel>();

            var result = await _bestRouteService.GetRangeRoutesAsync(getRoutesModel, cancellationToken)
                .ConfigureAwait(false);

            if (result == null || result.Count == 0)
            {
                return new Output<List<GetRouteResponseDto>>(new(), false)
                    .AddErrorMessage(ErrorMessages.NoRoutesFound);
            }

            var responseList = new List<GetRouteResponseDto>();
            foreach (var route in result)
            {
                var response = route.Adapt<GetRouteResponseDto>();
                responseList.Add(response);
            }

            return new Output<List<GetRouteResponseDto>>(responseList, true)
                .AddMessage(SuccessMessages.RoutesSearch);
        }

        public async Task<Output<GetRouteResponseDto>> GetBestRouteAsync(GetRoutesRequestDto request, CancellationToken cancellationToken)
        {
            var getRoutesModel = request.Adapt<GetRoutesModel>();

            var result = await _bestRouteService.GetBestRouteAsync(getRoutesModel, cancellationToken)
                .ConfigureAwait(false);

            if (result == null)
            {
                return new Output<GetRouteResponseDto>(new(), false)
                    .AddErrorMessage(ErrorMessages.NoRoutesFound);
            }

            var response = result.Adapt<GetRouteResponseDto>();

            return new Output<GetRouteResponseDto>(response, true)
                .AddMessage(SuccessMessages.BestRouteSearch);
        }

        public async Task<Output<GetRouteResponseDto>> CreateRouteAsync(CreateRouteRequestDto request, CancellationToken cancellationToken)
        {
            var createRouteModel = request.Adapt<CreateRouteModel>();

            var createdRoute = await _bestRouteService.CreateRouteAsync(createRouteModel, cancellationToken)
                .ConfigureAwait(false);

            if (createdRoute == null)
            {
                return new Output<GetRouteResponseDto>(new(), false)
                    .AddErrorMessage(ErrorMessages.RouteCreationFailed);
            }

            var response = createdRoute.Adapt<GetRouteResponseDto>();

            return new Output<GetRouteResponseDto>(response, true)
                .AddMessage(SuccessMessages.RouteCreated);
        }

        public async Task<Output<GetRouteResponseDto>> UpdateRouteAsync(Guid id, UpdateRouteRequestDto request, CancellationToken cancellationToken)
        {
            var updateRouteModel = request.Adapt<UpdateRouteModel>();
            updateRouteModel.Id = id;

            var updatedRoute = await _bestRouteService.UpdateRouteAsync(updateRouteModel, cancellationToken)
                .ConfigureAwait(false);

            if (updatedRoute == null)
            {
                return new Output<GetRouteResponseDto>(new(), false)
                    .AddErrorMessage(ErrorMessages.RouteNotFound);
            }

            var response = updatedRoute.Adapt<GetRouteResponseDto>();

            return new Output<GetRouteResponseDto>(response, true)
                .AddMessage(SuccessMessages.RouteUpdated);
        }

        public async Task<Output> DeleteRouteAsync(Guid id, CancellationToken cancellationToken)
        {
            var isSuccess = await _bestRouteService.DeleteRouteAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                return new Output(false)
                    .AddErrorMessage(ErrorMessages.RouteNotFound);
            }

            return new Output(true)
                .AddMessage(SuccessMessages.RouteDeleted);
        }
    }
}
