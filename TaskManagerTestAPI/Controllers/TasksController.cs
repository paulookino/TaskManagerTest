using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands;
using TaskManager.Application.Queries;

namespace TaskManagerTestAPI.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("ProjectId é obrigatório.");

            var tasks = await _mediator.Send(new GetTasksByProjectQuery { ProjectId = projectId });
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] CreateTaskCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.Title))
                return BadRequest("O título da tarefa é obrigatório.");

            command.ProjectId = projectId;

            var taskId = await _mediator.Send(command);
            return Ok(new { id = projectId });
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            if (command == null)
                return BadRequest("Dados inválidos.");

            command.TaskId = taskId;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            await _mediator.Send(new DeleteTaskCommand(taskId));
            return NoContent();
        }
    }

}
