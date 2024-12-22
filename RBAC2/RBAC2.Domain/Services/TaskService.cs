using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RBAC2.Database;
using RBAC2.Database.Entities;
using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly RbacDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(RbacDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskModel> CreateTaskAsync(TaskModel taskModel)
        {
            var taskEntity = _mapper.Map<Tasks>(taskModel);
            _context.Tasks.Add(taskEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskModel>(taskEntity);
        }

        public async Task<TaskModel> GetTaskByIdAsync(int taskId)
        {
            var taskEntity = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            return _mapper.Map<TaskModel>(taskEntity);
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            var taskEntities = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskModel>>(taskEntities);
        }

        public async Task<TaskModel> UpdateTaskAsync(TaskModel taskModel)
        {
            var taskEntity = await _context.Tasks.FindAsync(taskModel.TaskId);
            if (taskEntity == null)
            {
                return null;
            }

            _mapper.Map(taskModel, taskEntity);
            _context.Tasks.Update(taskEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskModel>(taskEntity);
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var taskEntity = await _context.Tasks.FindAsync(taskId);
            if (taskEntity == null)
            {
                return false;
            }

            _context.Tasks.Remove(taskEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
