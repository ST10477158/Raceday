using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class RouteViewModel
    {
        public int Route_ID { get; set; }
        public int Event_ID { get; set; }
        [Required] public string Route_Name { get; set; } = "";
        [Required] public decimal Route_Distance { get; set; }
        public string Route_Map { get; set; } = "";
        public IFormFile? MapImage { get; set; }
    }
}