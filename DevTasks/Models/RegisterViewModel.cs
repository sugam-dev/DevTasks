using System.ComponentModel.DataAnnotations;

namespace DevTasks.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Invalid Domain Name")]
        [StringLength(10, ErrorMessage = "Domain Name cannot be longer than 10 characters")]
        public string DomainName { get; set; }
    }

}
