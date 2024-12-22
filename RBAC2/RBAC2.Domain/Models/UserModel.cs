namespace RBAC2.Domain.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public bool CzyAktywny { get; set; }
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public string? IdentityUserId { get; set; }
    }
}
