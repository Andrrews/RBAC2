using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC2.Database.Entities
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Relacja one-to-many: jeden projekt może mieć wiele zadań
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

        public string CosProject { get; set; }
    }
}
