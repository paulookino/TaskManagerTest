namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface ITaskWriteRepository
    {
        Task AddAsync(Domain.Entities.Task task);
        Task UpdateAsync(Domain.Entities.Task task);
        Task DeleteAsync(Domain.Entities.Task task);
    }

}
