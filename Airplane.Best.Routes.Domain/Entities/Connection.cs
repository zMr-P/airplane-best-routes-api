using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Airplane.Best.Routes.Domain.Entities
{
    public class Connection
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Route")]
        public Guid RouteId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public Route Route { get; set; }
    }
}
