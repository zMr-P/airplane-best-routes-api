using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Dtos.RouteService.Response;
using Airplane.Best.Routes.Domain.Entities;
using Mapster;

namespace Airplane.Best.Routes.Application.Mapper
{
    public class MapsterProfile
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.NewConfig<CreateRouteRequestDto, Route>()
                .Map(dest => dest.OriginName, src => src.OriginName)
                .Map(dest => dest.DestinationName, src => src.DestinationName)
                .Map(dest => dest.IsAvaiable, src => src.IsAvaiable)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.Connections, src => src.Connections);

            config.NewConfig<Connection, GetConnectionResponseDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ConnectionName, src => src.Name);

            config.NewConfig<Route, GetRouteResponseDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.OriginName, src => src.OriginName)
                .Map(dest => dest.DestinationName, src => src.DestinationName)
                .Map(dest => dest.IsAvaiable, src => src.IsAvaiable)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.Connections, src => src.Connections);
        }
    }
}
