using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTasks.Models
{
    [Table("States")]
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only alphabets.")]
        public string Name { get; set; }

        // Navigation property for related districts
        public ICollection<District> Districts { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
