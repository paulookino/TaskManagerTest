using MediatR;
using TaskManager.Application.Dtos;
using TaskManager.Application.Queries;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Queries
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
    {
        private readonly IProjectReadRepository _projectReadRepository;

        public GetProjectsQueryHandler(IProjectReadRepository projectReadRepository)
        {
            _projectReadRepository = projectReadRepository;
        }

        public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectReadRepository.GetProjectsByUserAsync(request.UserId);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                TaskCount = p.Tasks.Count
            }).ToList();
        }
    }

}
