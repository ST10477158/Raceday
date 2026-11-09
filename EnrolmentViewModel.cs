namespace RaceDay.Web.ViewModels
{
    public class EnrolmentViewModel
    {
        public int Enrolment_ID { get; set; }
        public int Participant_ID { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; } = "";
        public decimal Category_EntryAmount { get; set; }
        public DateTime Enrolment_Date { get; set; }
        public int Race_Number { get; set; }
        public string Participant_Name { get; set; } = "";
        public int Event_ID { get; set; }
        public string Event_Name { get; set; } = "";
    }
}