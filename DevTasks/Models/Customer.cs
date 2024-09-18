using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTasks.Models
{
    [Table("Customers")]
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only alphabets.")]
        public string Name { get; set; }

        [Required]
        [Range(1, byte.MaxValue, ErrorMessage = "GenderId must be a number.")]
        public byte GenderId { get; set; }

        // Foreign key properties
        public int StateId { get; set; }
        public int DistrictId { get; set; }

        // Navigation properties
        public State State { get; set; }
        public District District { get; set; }
    }
}
