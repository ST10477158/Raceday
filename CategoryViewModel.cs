using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class CategoryViewModel
    {
        public int Category_ID { get; set; }
        public int Event_ID { get; set; }
        [Required] public string Category_Name { get; set; } = "";
        [Required] public decimal Category_EntryAmount { get; set; }
        [Required] public int Category_QualifyAge { get; set; }
    }
}