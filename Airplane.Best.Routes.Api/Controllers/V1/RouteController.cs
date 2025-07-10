using Airplane.Best.Routes.Application.Dtos;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
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
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<List<Airplane.Best.Routes.Domain.Entities.Route>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<List<Airplane.Best.Routes.Domain.Entities.Route>>))]
        [SwaggerOperation("Listar todas as rotas")]
        public async Task<IActionResult> GetAllRoutes(CancellationToken cancellationToken)
        {
            var routes = await _routeService.GetAllRoutesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (!routes.IsSuccess)
                return NotFound(routes);

            return Ok(routes);
        }

        [HttpGet]
        [Route("get-range")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<List<Airplane.Best.Routes.Domain.Entities.Route>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<List<Airplane.Best.Routes.Domain.Entities.Route>>))]
        [SwaggerOperation("Listar as rotas disponíveis referente a origem e destino")]
        public async Task<IActionResult> GetRoutesOriginDestination([FromQuery][Required] GetRoutesRequest request, CancellationToken cancellationToken)
        {
            var routes = await _routeService.GetRangeRoutesAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!routes.IsSuccess)
                return NotFound(routes);

            return Ok(routes);
        }

        [HttpGet]
        [Route("get-best")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [SwaggerOperation("Recuperar a rota de valor mais baixo disponível referente a origem e destino")]
        public async Task<IActionResult> GetBestRoute([FromQuery][Required] GetRoutesRequest request, CancellationToken cancellationToken)
        {
            var bestRoute = await _routeService.GetBestRouteAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!bestRoute.IsSuccess)
                return NotFound(bestRoute);

            return Ok(bestRoute);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [SwaggerOperation("Criar rota")]
        public async Task<IActionResult> CreateRoute([FromBody][Required] CreateRouteRequest request, CancellationToken cancellationToken)
        {
            var result = await _routeService.CreateRouteAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Output<Airplane.Best.Routes.Domain.Entities.Route>))]
        [SwaggerOperation("Atualizar rota")]
        public async Task<IActionResult> UpdateRoute([FromQuery][Required] Guid id, [FromBody][Required] CreateRouteRequest request)
        {
            var route = await _routeService.UpdateRouteAsync(id, request)
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
        public async Task<IActionResult> DeleteRoute([FromQuery][Required] Guid id)
        {
            var route = await _routeService.DeleteRouteAsync(id)
                .ConfigureAwait(false);

            if (!route.IsSuccess)
                return NotFound(route);

            return Ok(route);
        }
    }
}
