using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Participant
    {
        [Key]
        public int Participant_ID { get; set; }
        public int User_ID { get; set; }
        public User? User { get; set; }
        public string Participant_Name { get; set; } = string.Empty;
        public string Participant_Surname { get; set; } = string.Empty;
    }
}