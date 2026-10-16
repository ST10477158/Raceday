using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Organiser
    {
        [Key]
        public int Organiser_ID { get; set; }
        public int User_ID { get; set; }
        public User? User { get; set; }
        public string Organisation_Name { get; set; } = string.Empty;
        public string Organisation_Phone { get; set; } = string.Empty;
    }
}