using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskReadRepository : ITaskReadRepository
    {
        private readonly TaskManagerDbContext _context;

        public TaskReadRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Task> GetByIdAsync(Guid taskId)
        {
            return await _context.Tasks
                .Include(t => t.History)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
    }

}
