using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    /// <summary>
    /// Interface for project service operations.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectModel">The project model to create.</param>
        /// <returns>The created project model.</returns>
        Task<ProjectModel> CreateProjectAsync(ProjectModel projectModel);

        /// <summary>
        /// Gets a project by its ID.
        /// </summary>
        /// <param name="projectId">The ID of the project to retrieve.</param>
        /// <returns>The project model with the specified ID.</returns>
        Task<ProjectModel> GetProjectByIdAsync(int projectId);

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>A collection of all project models.</returns>
        Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="projectModel">The project model to update.</param>
        /// <returns>The updated project model.</returns>
        Task<ProjectModel> UpdateProjectAsync(ProjectModel projectModel);

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="projectId">The ID of the project to delete.</param>
        /// <returns>True if the project was deleted, otherwise false.</returns>
        Task<bool> DeleteProjectAsync(int projectId);
    }
}
