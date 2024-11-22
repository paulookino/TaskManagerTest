using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class UpdateTaskCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Update_Task_Details_And_Status_When_Command_Is_Valid()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new Domain.Entities.Task("Old Title", "Old Description", DateTime.UtcNow.AddDays(-5), EPriority.Medium, Guid.NewGuid())
            {
                Id = taskId,
                Status = EStatus.Pending
            };

            var command = new UpdateTaskCommand
            {
                TaskId = taskId,
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = EStatus.Completed
            };

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(existingTask);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            mockTaskWriteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new UpdateTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.Task>(t =>
                t.Title == command.Title &&
                t.Description == command.Description &&
                t.DueDate == command.DueDate &&
                t.Status == command.Status
            )), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Task_Not_Found()
        {
            // Arrange
            var command = new UpdateTaskCommand
            {
                TaskId = Guid.NewGuid(),
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = EStatus.Completed
            };

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Task)null);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();

            var handler = new UpdateTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Tarefa não encontrada.");

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Task>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Update_Only_Status_When_Details_Are_Not_Provided()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new Domain.Entities.Task("Old Title", "Old Description", DateTime.UtcNow.AddDays(-5), EPriority.Medium, Guid.NewGuid())
            {
                Id = taskId,
                Status = EStatus.Pending
            };

            var command = new UpdateTaskCommand
            {
                TaskId = taskId,
                Status = EStatus.Completed
            };

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(existingTask);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            mockTaskWriteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new UpdateTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.Task>(t =>
                t.Status == command.Status &&
                t.Title == existingTask.Title &&
                t.Description == existingTask.Description &&
                t.DueDate == existingTask.DueDate
            )), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Not_Update_If_No_Changes_Are_Provided()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new Domain.Entities.Task("Old Title", "Old Description", DateTime.UtcNow.AddDays(-5), EPriority.Medium, Guid.NewGuid())
            {
                Id = taskId,
                Status = EStatus.Pending
            };

            var command = new UpdateTaskCommand
            {
                TaskId = taskId
            };

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(existingTask);

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();

            var handler = new UpdateTaskCommandHandler(mockTaskWriteRepository.Object, mockTaskReadRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            mockTaskReadRepository.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            mockTaskWriteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Task>()), Times.Once);
        }
    }
}