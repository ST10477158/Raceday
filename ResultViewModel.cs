namespace RaceDay.Web.ViewModels
{
    public class ResultViewModel
    {
        public int Results_ID { get; set; }
        public int Enrolment_ID { get; set; }
        public TimeSpan? Finish_Time { get; set; }
        public int? Finish_Position { get; set; }
        public decimal? Average_Pace { get; set; }
        public int? Category_Position { get; set; }
        public string Event_Name { get; set; } = "";
        public int Event_ID { get; set; }
    }
}