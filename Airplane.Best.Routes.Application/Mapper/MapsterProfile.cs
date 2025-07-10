using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Domain.Entities;
using Mapster;

namespace Airplane.Best.Routes.Application.Mapper
{
    public class MapsterProfile
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.NewConfig<CreateRouteRequest, Route>()
                .Map(dest => dest.OriginName, src => src.OriginName)
                .Map(dest => dest.DestinationName, src => src.DestinationName)
                .Map(dest => dest.IsAvaiable, src => src.IsAvaiable)
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.Connections, src => src.Connections);
        }
    }
}
