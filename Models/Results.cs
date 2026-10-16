using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Results
    {
        [Key]
        public int Results_ID { get; set; }
        public int Enrolment_ID { get; set; }
        public Enrolment? Enrolment { get; set; }
        public TimeSpan? Finish_Time { get; set; }
        public int? Finish_Position { get; set; }
        public decimal? Average_Pace { get; set; }
        public int? Category_Position { get; set; }
    }
}