using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTasks.Models
{
    [Table("Customers")]
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only alphabets.")]
        public string Name { get; set; }

        [Required]
        [Range(1, byte.MaxValue, ErrorMessage = "GenderId must be a number.")]
        public byte GenderId { get; set; }

        [Required]
        [Range(1, short.MaxValue, ErrorMessage = "StateId must be a number.")]
        public short StateId { get; set; }

        [Required]
        [Range(1, short.MaxValue, ErrorMessage = "DistrictId must be a number.")]
        public short DistrictId { get; set; }
    }
}
