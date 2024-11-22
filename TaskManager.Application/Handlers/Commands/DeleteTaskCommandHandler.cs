using MediatR;
using TaskManager.Application.Commands;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskWriteRepository _taskWriteRepository;
        private readonly ITaskReadRepository _taskReadRepository;

        public DeleteTaskCommandHandler(
            ITaskWriteRepository taskWriteRepository,
            ITaskReadRepository taskReadRepository)
        {
            _taskWriteRepository = taskWriteRepository;
            _taskReadRepository = taskReadRepository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskReadRepository.GetByIdAsync(request.TaskId);
            if (task == null)
            {
                throw new ArgumentException("Tarefa não encontrada.");
            }

            if (task.Status != EStatus.Completed)
            {
                throw new InvalidOperationException("Apenas tarefas concluídas podem ser removidas.");
            }

           await _taskWriteRepository.DeleteAsync(task);

            return Unit.Value;
        }
    }


}
