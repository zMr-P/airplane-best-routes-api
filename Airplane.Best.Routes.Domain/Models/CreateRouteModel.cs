namespace Airplane.Best.Routes.Domain.Models
{
    public class CreateRouteModel
    {
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public bool IsAvaiable { get; set; }
        public decimal Value { get; set; }
        public List<CreateConnectionModel> Connections { get; set; }
    }
}
