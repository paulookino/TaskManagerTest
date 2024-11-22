using MediatR;
using TaskManager.Application.Dtos;
using TaskManager.Application.Queries;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Queries
{
    public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, List<TaskDto>>
    {
        private readonly ITaskReadRepository _taskReadRepository;

        public GetTasksByProjectQueryHandler(ITaskReadRepository taskReadRepository)
        {
            _taskReadRepository = taskReadRepository;
        }

        public async Task<List<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskReadRepository.GetTasksByProjectAsync(request.ProjectId);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString()
            }).ToList();
        }
    }
}
