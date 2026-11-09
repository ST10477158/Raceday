using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress] public string User_Email { get; set; } = "";
        [Required, DataType(DataType.Password)] public string User_Password { get; set; } = "";
        [Required] public string User_Role { get; set; } = "Participant";
    }
}