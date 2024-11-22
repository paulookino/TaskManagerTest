using MediatR;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Commands
{
    public class CreateTaskCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public EPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
    }

}
