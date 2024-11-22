using FluentAssertions;
using Moq;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.Application.Handlers.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Create_Project_And_Return_Id_When_Command_Is_Valid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Name = "New Project",
                Description = "Project Description",
                UserId = userId
            };

            var mockProjectWriteRepository = new Mock<IProjectWriteRepository>();
            mockProjectWriteRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new CreateProjectCommandHandler(mockProjectWriteRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            mockProjectWriteRepository.Verify(repo =>
                repo.AddAsync(It.Is<Project>(p =>
                    p.Name == command.Name &&
                    p.Description == command.Description &&
                    p.UserId == command.UserId
                )),
                Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_Throw_Exception_When_Repository_Fails()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                Name = "New Project",
                Description = "Project Description",
                UserId = Guid.NewGuid()
            };

            var mockProjectWriteRepository = new Mock<IProjectWriteRepository>();
            mockProjectWriteRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new CreateProjectCommandHandler(mockProjectWriteRepository.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");

            mockProjectWriteRepository.Verify(repo =>
                repo.AddAsync(It.IsAny<Project>()),
                Times.Once);
        }
    }
}