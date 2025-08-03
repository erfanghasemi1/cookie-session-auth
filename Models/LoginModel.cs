using System.ComponentModel.DataAnnotations;

namespace Cookie_Session.Models
{
    public class LoginModel
    {
        public string? Id { get; set; }
        [Required]
        public string? Username { get; set; }

        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
