using System.ComponentModel.DataAnnotations;

namespace Airplane.Best.Routes.Domain.Entities
{
    public class Route
    {
        [Key]
        public Guid Id { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public bool IsAvaiable { get; set; }
        public decimal Value { get; set; }
        public List<Connection> Connections { get; set; }
    }
}
