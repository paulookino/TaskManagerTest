using MediatR;

namespace TaskManager.Application.Commands
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }

}
