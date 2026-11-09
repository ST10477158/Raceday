using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class ProfileViewModel
    {
        public int User_ID { get; set; }
        [Required, EmailAddress] public string User_Email { get; set; } = "";
        public string? User_Password { get; set; }
        public string DisplayName { get; set; } = "";
    }
}