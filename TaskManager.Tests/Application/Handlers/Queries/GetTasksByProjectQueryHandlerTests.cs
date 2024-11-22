using FluentAssertions;
using Moq;
using TaskManager.Application.Handlers.Queries;
using TaskManager.Application.Queries;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;


namespace TaskManager.Tests.Application.Handlers.Queries
{
    public class GetTasksByProjectQueryHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Return_List_Of_TaskDtos_When_Tasks_Exist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskManager.Domain.Entities.Task>
        {
            new TaskManager.Domain.Entities.Task("Task 1", "Description 1", DateTime.UtcNow.AddDays(1), EPriority.High, projectId)
            {
                Id = Guid.NewGuid(),
                Status = EStatus.InProgress
            },
            new TaskManager.Domain.Entities.Task("Task 2", "Description 2", DateTime.UtcNow.AddDays(2), EPriority.Low, projectId)
            {
                Id = Guid.NewGuid(),
                Status = EStatus.Pending
            }
        };

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository
                .Setup(repo => repo.GetTasksByProjectAsync(projectId))
                .ReturnsAsync(tasks);

            var handler = new GetTasksByProjectQueryHandler(mockTaskReadRepository.Object);
            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Id.Should().Be(tasks[0].Id);
            result[0].Title.Should().Be(tasks[0].Title);
            result[0].Description.Should().Be(tasks[0].Description);
            result[0].DueDate.Should().Be(tasks[0].DueDate);
            result[0].Status.Should().Be(tasks[0].Status.ToString());
            result[0].Priority.Should().Be(tasks[0].Priority.ToString());

            result[1].Id.Should().Be(tasks[1].Id);
            result[1].Title.Should().Be(tasks[1].Title);
            result[1].Description.Should().Be(tasks[1].Description);
            result[1].DueDate.Should().Be(tasks[1].DueDate);
            result[1].Status.Should().Be(tasks[1].Status.ToString());
            result[1].Priority.Should().Be(tasks[1].Priority.ToString());
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Return_Empty_List_When_No_Tasks_Exist()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository
                .Setup(repo => repo.GetTasksByProjectAsync(projectId))
                .ReturnsAsync(new List<TaskManager.Domain.Entities.Task>());

            var handler = new GetTasksByProjectQueryHandler(mockTaskReadRepository.Object);
            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Repository_Throws_Exception()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            var mockTaskReadRepository = new Mock<ITaskReadRepository>();
            mockTaskReadRepository
                .Setup(repo => repo.GetTasksByProjectAsync(projectId))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new GetTasksByProjectQueryHandler(mockTaskReadRepository.Object);
            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
    }
}