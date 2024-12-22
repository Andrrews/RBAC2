using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAC2.Database.Entities
{
    [Table("User", Schema = "own")]
    public class User 
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        public bool CzyAktywny { get; set; }

        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
        // Relacja z IdentityUser
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }
    }
}
