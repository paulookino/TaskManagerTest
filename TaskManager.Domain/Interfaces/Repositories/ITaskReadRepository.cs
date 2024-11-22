namespace TaskManager.Domain.Interfaces.Repositories
{
    public interface ITaskReadRepository
    {
        Task<List<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId);
        Task<Domain.Entities.Task> GetByIdAsync(Guid taskId);
    }

}
