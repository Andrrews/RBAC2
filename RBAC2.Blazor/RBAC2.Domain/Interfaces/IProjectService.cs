using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    public interface IProjectService
    {
        Task<ProjectModel> CreateProjectAsync(ProjectModel projectModel);
        Task<ProjectModel> GetProjectByIdAsync(int projectId);
        Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
        Task<ProjectModel> UpdateProjectAsync(ProjectModel projectModel);
        Task<bool> DeleteProjectAsync(int projectId);
    }
}
