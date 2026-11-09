using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class EventViewModel
    {
        public int Event_ID { get; set; }
        public int Organiser_ID { get; set; }
        [Required] public string Event_Name { get; set; } = "";
        [Required] public string Event_Description { get; set; } = "";
        [Required] public string Event_Venue { get; set; } = "";
        [Required] public DateTime Event_StartTime { get; set; } = DateTime.Now;
    }
}