using MediatR;
using TaskManager.Application.Dtos;

namespace TaskManager.Application.Queries
{
    public class GetTasksByProjectQuery : IRequest<List<TaskDto>>
    {
        public Guid ProjectId { get; set; }
    }
}
