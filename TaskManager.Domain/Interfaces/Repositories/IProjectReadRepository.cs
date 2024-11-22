using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface IProjectReadRepository
    {
        Task<List<Project>> GetProjectsByUserAsync(Guid userId);
        Task<Project> GetByIdAsync(Guid projectId);
    }

}
