using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;


namespace TaskManager.Tests.Application.Handlers.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Create_Task_And_Return_Id_When_Command_Is_Valid()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var command = new CreateTaskCommand
            {
                Title = "New Task",
                Description = "Task Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Priority = EPriority.Medium,
                ProjectId = projectId
            };

            var project = new Project("Test Project", "Test Description", Guid.NewGuid())
            {
                Id = projectId,
                Tasks = new List<Domain.Entities.Task>()
            };

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            mockTaskWriteRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);

            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            var handler = new CreateTaskCommandHandler(mockTaskWriteRepository.Object, mockProjectReadRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            mockTaskWriteRepository.Verify(repo =>
                repo.AddAsync(It.Is<Domain.Entities.Task>(t =>
                    t.Title == command.Title &&
                    t.Description == command.Description &&
                    t.DueDate == command.DueDate &&
                    t.Priority == command.Priority &&
                    t.ProjectId == command.ProjectId
                )),
                Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Project_Not_Found()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "New Task",
                Description = "Task Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Priority = EPriority.Medium,
                ProjectId = Guid.NewGuid()
            };

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project)null);

            var handler = new CreateTaskCommandHandler(mockTaskWriteRepository.Object, mockProjectReadRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Projeto não encontrado.");
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Project_Has_Maximum_Tasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var command = new CreateTaskCommand
            {
                Title = "New Task",
                Description = "Task Description",
                DueDate = DateTime.UtcNow.AddDays(5),
                Priority = EPriority.Medium,
                ProjectId = projectId
            };

            var project = new Project("Test Project", "Test Description", Guid.NewGuid())
            {
                Id = projectId,
                Tasks = new List<Domain.Entities.Task>(new Domain.Entities.Task[20]) // Simulate 20 tasks already present
            };

            var mockTaskWriteRepository = new Mock<ITaskWriteRepository>();
            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            var handler = new CreateTaskCommandHandler(mockTaskWriteRepository.Object, mockProjectReadRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("O projeto atingiu o limite máximo de 20 tarefas.");
        }
    }
}