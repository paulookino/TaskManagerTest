using FluentAssertions;
using Moq;
using TaskManager.Application.Handlers.Queries;
using TaskManager.Application.Queries;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.Application.Handlers.Queries
{
    public class GetProjectsQueryHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Return_List_Of_ProjectDtos_When_Projects_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<Project>
        {
            new Project("Project 1", "Description 1", userId),
            new Project("Project 2", "Description 2", userId)
        };

            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository
                .Setup(repo => repo.GetProjectsByUserAsync(userId))
                .ReturnsAsync(projects);

            var handler = new GetProjectsQueryHandler(mockProjectReadRepository.Object);
            var query = new GetProjectsQuery { UserId = userId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Id.Should().Be(projects[0].Id);
            result[0].Name.Should().Be(projects[0].Name);
            result[0].Description.Should().Be(projects[0].Description);
            result[0].TaskCount.Should().Be(2);

            result[1].Id.Should().Be(projects[1].Id);
            result[1].Name.Should().Be(projects[1].Name);
            result[1].Description.Should().Be(projects[1].Description);
            result[1].TaskCount.Should().Be(1);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Return_Empty_List_When_No_Projects_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository
                .Setup(repo => repo.GetProjectsByUserAsync(userId))
                .ReturnsAsync(new List<Project>());

            var handler = new GetProjectsQueryHandler(mockProjectReadRepository.Object);
            var query = new GetProjectsQuery { UserId = userId };

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
            var userId = Guid.NewGuid();

            var mockProjectReadRepository = new Mock<IProjectReadRepository>();
            mockProjectReadRepository
                .Setup(repo => repo.GetProjectsByUserAsync(userId))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new GetProjectsQueryHandler(mockProjectReadRepository.Object);
            var query = new GetProjectsQuery { UserId = userId };

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
    }
}