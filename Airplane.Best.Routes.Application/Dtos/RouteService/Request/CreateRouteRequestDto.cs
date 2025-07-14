namespace Airplane.Best.Routes.Application.Dtos.RouteService.Request
{
    public class CreateRouteRequestDto
    {
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public bool IsAvaiable { get; set; }
        public decimal Value { get; set; }
        public List<CreateConnectionRequestDto>? Connections { get; set; }
    }

    public class CreateConnectionRequestDto
    {
        public string Name { get; set; }
    }
}
