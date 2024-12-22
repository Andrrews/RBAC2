using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    public interface ITaskService
    {
        Task<TaskModel> CreateTaskAsync(TaskModel taskModel);
        Task<TaskModel> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<TaskModel> UpdateTaskAsync(TaskModel taskModel);
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
