using System.ComponentModel.DataAnnotations;
namespace Raceday.Models
{
    public class Enrolment
    {
        [Key]
        public int Enrolment_ID { get; set; }
        public int Participant_ID { get; set; }
        public Participant? Participant { get; set; }
        public int Category_ID { get; set; }
        public Category? Category { get; set; }
        public DateTime Enrolment_Date { get; set; }
        public int Race_Number { get; set; }
    }
}