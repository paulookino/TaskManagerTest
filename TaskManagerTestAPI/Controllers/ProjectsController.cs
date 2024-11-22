using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands;
using TaskManager.Application.Queries;

namespace TaskManagerTestAPI.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("UserId é obrigatório.");

            var projects = await _mediator.Send(new GetProjectsQuery { UserId = userId });
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.Name))
                return BadRequest("Nome do projeto é obrigatório.");

            var projectId = await _mediator.Send(command);
            return Ok(new { id = projectId });
        }
    }

}
