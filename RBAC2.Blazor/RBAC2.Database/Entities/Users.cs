using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC2.Database.Entities
{
    public class User 
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        public bool CzyAktywny { get; set; }

        public string CosUser { get; set; }


        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
        // Relacja z IdentityUser
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }
    }
}
