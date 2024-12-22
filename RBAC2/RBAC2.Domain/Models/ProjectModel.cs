namespace RBAC2.Domain.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
