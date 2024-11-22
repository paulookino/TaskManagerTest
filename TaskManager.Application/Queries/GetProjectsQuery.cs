using MediatR;
using TaskManager.Application.Dtos;

namespace TaskManager.Application.Queries
{
    public class GetProjectsQuery : IRequest<List<ProjectDto>>
    {
        public Guid UserId { get; set; }
    }

}
