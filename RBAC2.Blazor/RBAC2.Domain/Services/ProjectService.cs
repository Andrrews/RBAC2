using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RBAC2.Database;
using RBAC2.Database.Entities;
using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    public class ProjectService : IProjectService
    {
        private readonly RbacDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(RbacDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectModel> CreateProjectAsync(ProjectModel projectModel)
        {
            var projectEntity = _mapper.Map<Project>(projectModel);
            _context.Projects.Add(projectEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectModel>(projectEntity);
        }

        public async Task<ProjectModel> GetProjectByIdAsync(int projectId)
        {
            var projectEntity = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            return _mapper.Map<ProjectModel>(projectEntity);
        }

        public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
        {
            var projectEntities = await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectModel>>(projectEntities);
        }

        public async Task<ProjectModel> UpdateProjectAsync(ProjectModel projectModel)
        {
            var projectEntity = await _context.Projects.FindAsync(projectModel.ProjectId);
            if (projectEntity == null)
            {
                return null;
            }

            _mapper.Map(projectModel, projectEntity);
            _context.Projects.Update(projectEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectModel>(projectEntity);
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var projectEntity = await _context.Projects.FindAsync(projectId);
            if (projectEntity == null)
            {
                return false;
            }

            _context.Projects.Remove(projectEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
