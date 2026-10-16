using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class Category
    {
        [Key]
        public int Category_ID { get; set; }
        public int Event_ID { get; set; }
        public Events? Event { get; set; }
        public string Category_Name { get; set; } = string.Empty;
        public decimal Category_EntryAmount { get; set; }
        public int Category_QualifyAge { get; set; }
    }
}