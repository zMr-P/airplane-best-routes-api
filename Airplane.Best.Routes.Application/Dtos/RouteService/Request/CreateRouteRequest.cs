namespace Airplane.Best.Routes.Application.Dtos.RouteService.Request
{
    public class CreateRouteRequest
    {
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public bool IsAvaiable { get; set; }
        public decimal Value { get; set; }
        public List<CreateConnectionRequest> Connections { get; set; }
    }

    public class CreateConnectionRequest
    {
        public string Name { get; set; }
    }
}
