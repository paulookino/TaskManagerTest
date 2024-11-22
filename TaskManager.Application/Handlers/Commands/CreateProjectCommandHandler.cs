using MediatR;
using TaskManager.Application.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Handlers.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectWriteRepository _projectWriteRepository;

        public CreateProjectCommandHandler(IProjectWriteRepository projectWriteRepository)
        {
            _projectWriteRepository = projectWriteRepository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // Criação da entidade
            var project = new Project(request.Name, request.Description, request.UserId);

            // Persistência no banco
            await _projectWriteRepository.AddAsync(project);

            return project.Id;
        }
    }

}
