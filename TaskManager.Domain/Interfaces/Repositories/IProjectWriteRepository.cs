using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface IProjectWriteRepository
    {
        System.Threading.Tasks.Task AddAsync(Project project);
    }

}
