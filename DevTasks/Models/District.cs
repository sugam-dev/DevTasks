using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevTasks.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key for State
        public int StateId { get; set; }

        // Navigation property for State
        public State State { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
