using MediatR;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Commands
{
    public class UpdateTaskCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public EStatus? Status { get; set; }
    }
}
