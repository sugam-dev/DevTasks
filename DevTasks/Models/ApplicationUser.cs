using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DevTasks
{
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Invalid Domain Name")]
        [StringLength(10, ErrorMessage = "Domain Name cannot be longer than 10 characters")]
        public string DomainName { get; set; }
    }
}
