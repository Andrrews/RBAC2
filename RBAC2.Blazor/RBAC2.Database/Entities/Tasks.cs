using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC2.Database.Entities
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
        public string CosTask { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }

        public User User { get; set; }


        [ForeignKey("Project")] public int ProjectId { get; set; }

        public Project Project { get; set; }


    }
}
