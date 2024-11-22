using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectWriteRepository : IProjectWriteRepository
    {
        private readonly TaskManagerDbContext _context;

        public ProjectWriteRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
    }


}
