namespace Airplane.Best.Routes.Application.Dtos.RouteService.Response
{
    public class GetRouteResponseDto
    {
        public Guid Id { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public bool IsAvaiable { get; set; }
        public decimal Value { get; set; }
        public List<GetConnectionResponseDto> Connections { get; set; }
    }
    public class GetConnectionResponseDto
    {
        public Guid Id { get; set; }
        public string ConnectionName { get; set; }
    }
}
