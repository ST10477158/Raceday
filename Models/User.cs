using System.ComponentModel.DataAnnotations;

namespace Raceday.Models
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        public string User_Email { get; set; } = string.Empty;
        public string User_Password { get; set; } = string.Empty;
        public string User_Role { get; set; } = string.Empty;
    }
}