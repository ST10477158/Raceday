using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Route
    {
        [Key]
        public int Route_ID { get; set; }
        public int Event_ID { get; set; }
        public Events? Event { get; set; }
        public string Route_Name { get; set; } = string.Empty;
        public decimal Route_Distance { get; set; }
        public string Route_Map { get; set; } = string.Empty;
    }
}