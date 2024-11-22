using MediatR;
using TaskManager.Application.Commands;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskWriteRepository _taskWriteRepository;
        private readonly ITaskReadRepository _taskReadRepository;

        public UpdateTaskCommandHandler(ITaskWriteRepository taskWriteRepository, ITaskReadRepository taskReadRepository)
        {
            _taskWriteRepository = taskWriteRepository;
            _taskReadRepository = taskReadRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskReadRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new ArgumentException("Tarefa não encontrada.");

            if (!string.IsNullOrEmpty(request.Title) && !string.IsNullOrEmpty(request.Description) && request.DueDate != default)
            {
                task.UpdateDetails(request.Title, request.Description, request.DueDate);
            }

            if (request.Status.HasValue)
            {
                task.UpdateStatus(request.Status.Value);
            }

            await _taskWriteRepository.UpdateAsync(task);

            return Unit.Value;
        }
    }

}
