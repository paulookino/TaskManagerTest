using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectReadRepository : IProjectReadRepository
    {
        private readonly TaskManagerDbContext _context;

        public ProjectReadRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsByUserAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Project> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }
    }

}
