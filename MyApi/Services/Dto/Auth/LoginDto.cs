using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class LoginDto
    {
        [Required] 
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required] 
        [Display(Name = "Password")]
        [StringLength(40, ErrorMessage = "Password is too short (minimum is 8 characters).", MinimumLength =8)]
        public string Password { get; set; }
    }
}