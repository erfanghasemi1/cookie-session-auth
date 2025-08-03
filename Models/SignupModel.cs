using System.ComponentModel.DataAnnotations;

namespace Cookie_Session.Models
{
    public class SignupModel
    {
        public string? UserId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [MinLength(8)]
        public string? Password { get; set; }

        [RegularExpression(@"^09\d{9}$" , ErrorMessage ="Phone number is Invalid!")]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

    }
}
