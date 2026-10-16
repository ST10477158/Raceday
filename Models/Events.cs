using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Events
    {
        [Key]
        public int Event_ID { get; set; }
        public int Organiser_ID { get; set; }
        public Organiser? Organiser { get; set; }
        public string Event_Name { get; set; } = string.Empty;
        public string Event_Description { get; set; } = string.Empty;
        public string Event_Venue { get; set; } = string.Empty;
        public DateTime Event_StartTime { get; set; }
    }
}