using MediatR;
using TaskManager.Application.Commands;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskWriteRepository _taskWriteRepository;
        private readonly IProjectReadRepository _projectReadRepository;

        public CreateTaskCommandHandler(
            ITaskWriteRepository taskWriteRepository,
            IProjectReadRepository projectReadRepository)
        {
            _taskWriteRepository = taskWriteRepository;
            _projectReadRepository = projectReadRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectReadRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
                throw new ArgumentException("Projeto não encontrado.");

            if (project.Tasks.Count >= 20)
                throw new InvalidOperationException("O projeto atingiu o limite máximo de 20 tarefas.");

            var task = new Domain.Entities.Task(
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority,
                request.ProjectId);

            await _taskWriteRepository.AddAsync(task);

            return task.Id;
        }
    }

}
