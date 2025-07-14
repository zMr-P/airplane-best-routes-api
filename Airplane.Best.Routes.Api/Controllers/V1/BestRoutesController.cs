using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Dtos.RouteService.Response;
using Airplane.Best.Routes.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace airplane_best_routes_api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion(1.0)]
    public class BestRoutesController : ControllerBase
    {
        private readonly IBestRoutesApplication _bestRoutesApplication;
        private readonly ILogger<BestRoutesController> _logger;
        public BestRoutesController(IBestRoutesApplication bestRoutesApplication, ILogger<BestRoutesController> logger)
        {
            _bestRoutesApplication = bestRoutesApplication;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<List<GetRouteResponseDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<List<GetRouteResponseDto>>))]
        [SwaggerOperation("Listar todas as rotas")]
        public async Task<IActionResult> GetAllRoutes(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: get-all, trace: {ControllerContext.ActionDescriptor.DisplayName}");

            var routes = await _bestRoutesApplication.GetAllRoutesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (!routes.IsSuccess)
                return NotFound(routes);

            return Ok(routes);
        }

        [HttpGet]
        [Route("get-range")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<List<GetRouteResponseDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<List<GetRouteResponseDto>>))]
        [SwaggerOperation("Listar as rotas disponíveis referente a origem e destino")]
        public async Task<IActionResult> GetRoutesOriginDestination([FromQuery][Required] GetRoutesRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: get-range, trace: {ControllerContext.ActionDescriptor.DisplayName}");

            var routes = await _bestRoutesApplication.GetRangeRoutesAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!routes.IsSuccess)
                return NotFound(routes);

            return Ok(routes);
        }

        [HttpGet]
        [Route("get-best")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<GetRouteResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<GetRouteResponseDto>))]
        [SwaggerOperation("Recuperar a rota de valor mais baixo disponível referente a origem e destino")]
        public async Task<IActionResult> GetBestRoute([FromQuery][Required] GetRoutesRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: get-best, trace: {ControllerContext.ActionDescriptor.DisplayName}");

            var bestRoute = await _bestRoutesApplication.GetBestRouteAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!bestRoute.IsSuccess)
                return NotFound(bestRoute);

            return Ok(bestRoute);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<GetRouteResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Output<GetRouteResponseDto>))]
        [SwaggerOperation("Criar rota")]
        public async Task<IActionResult> CreateRoute([FromBody][Required] CreateRouteRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: create, trace: {ControllerContext.ActionDescriptor.DisplayName}");
            var result = await _bestRoutesApplication.CreateRouteAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<GetRouteResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<GetRouteResponseDto>))]
        [SwaggerOperation("Atualizar rota")]
        public async Task<IActionResult> UpdateRoute([FromQuery][Required] Guid id,
            [FromBody][Required] UpdateRouteRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: update, trace: {ControllerContext.ActionDescriptor.DisplayName}");

            var route = await _bestRoutesApplication.UpdateRouteAsync(id, request, cancellationToken)
                .ConfigureAwait(false);

            if (!route.IsSuccess)
                return NotFound(route);

            return Ok(route);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output))]
        [SwaggerOperation("Deletar rota")]
        public async Task<IActionResult> DeleteRoute([FromQuery][Required] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Chamando endpoint: delete, trace: {ControllerContext.ActionDescriptor.DisplayName}");

            var route = await _bestRoutesApplication.DeleteRouteAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (!route.IsSuccess)
                return NotFound(route);

            return Ok(route);
        }
    }
}
