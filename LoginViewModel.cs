using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required, EmailAddress] public string User_Email { get; set; } = "";
        [Required, DataType(DataType.Password)] public string User_Password { get; set; } = "";
    }
}