using FluentAssertions;
using MediatR;
using Moq;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.Application.Handlers.Commands
{
    public class DeleteTaskCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Delete_Task_When_Task_Is_Completed()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var completedTask = new Domain.Entities.Task("Test Task", "Description", DateTime.UtcNow, EPriority.Medium, Guid.NewGuid())
            {
                Id = taskId,
                Status = EStatus.Completed
            };

            var command = new DeleteTaskCommand(taskId);

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(completedTask);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            mockTaskWriteRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new DeleteTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.DeleteAsync(completedTask), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Task_Not_Found()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.NewGuid());

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Task)null);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();

            var handler = new DeleteTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Tarefa não encontrada.");

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Task>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Task_Is_Not_Completed()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var pendingTask = new Domain.Entities.Task("Test Task", "Description", DateTime.UtcNow, EPriority.Medium, Guid.NewGuid())
            {
                Id = taskId,
                Status = EStatus.Pending
            };

            var command = new DeleteTaskCommand(taskId);

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(pendingTask);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();

            var handler = new DeleteTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Apenas tarefas concluídas podem ser removidas.");

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Task>()), Times.Never);
        }
    }

}