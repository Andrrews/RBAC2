using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    /// <summary>
    /// Interface for task service operations.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Creates a new task asynchronously.
        /// </summary>
        /// <param name="taskModel">The task model to create.</param>
        /// <returns>The created task model.</returns>
        Task<TaskModel> CreateTaskAsync(TaskModel taskModel);

        /// <summary>
        /// Gets a task by its ID asynchronously.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <returns>The task model with the specified ID.</returns>
        Task<TaskModel> GetTaskByIdAsync(int taskId);

        /// <summary>
        /// Gets all tasks asynchronously.
        /// </summary>
        /// <returns>A collection of all task models.</returns>
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();

        /// <summary>
        /// Updates an existing task asynchronously.
        /// </summary>
        /// <param name="taskModel">The task model to update.</param>
        /// <returns>The updated task model.</returns>
        Task<TaskModel> UpdateTaskAsync(TaskModel taskModel);

        /// <summary>
        /// Deletes a task by its ID asynchronously.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete.</param>
        /// <returns>True if the task was deleted successfully, otherwise false.</returns>
        Task<bool> DeleteTaskAsync(int taskId);
    }
}
