namespace RBAC2.Domain.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
