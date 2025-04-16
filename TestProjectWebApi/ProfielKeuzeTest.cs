using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace TestProjectWebApi
{
    [TestClass]
    public class EnvironmentControllerTests
    {
        private readonly Mock<IEnvironmentRepository> _mockRepo;
        private readonly Mock<ILogger<EnvironmentController>> _mockLogger;
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly EnvironmentController _controller;

        public EnvironmentControllerTests()
        {
            _mockRepo = new Mock<IEnvironmentRepository>();
            _mockLogger = new Mock<ILogger<EnvironmentController>>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _controller = new EnvironmentController(_mockRepo.Object, _mockLogger.Object, _mockAuthService.Object);
        }

        [TestMethod]
        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns((string)null);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [TestMethod]
        [Fact]
        public async Task Get_ReturnsOk_WithProfielKeuzes()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var profielKeuzes = new List<ProjectMap.WebApi.Models.Environment> { new ProjectMap.WebApi.Models.Environment { Id = Guid.NewGuid() } };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetEnvironmentsByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(profielKeuzes);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(profielKeuzes, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Get_WithId_ReturnsNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync<IEnvironmentRepository, ProjectMap.WebApi.Models.Environment>((ProjectMap.WebApi.Models.Environment)null);

            // Act
            var result = await _controller.Get(environmentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [TestMethod]
        [Fact]
        public async Task Get_WithId_ReturnsOk_WithEnvironment()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            var environments = new ProjectMap.WebApi.Models.Environment { Id = environmentId };
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync(environments);

            // Act
            var result = await _controller.Get(environmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(environments, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns((string)null);

            // Act
            var result = await _controller.Add(new ProjectMap.WebApi.Models.Environment());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsBadRequest_WhenUserHasMaxProfielKeuzes()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var existingEnvironments = new List<ProjectMap.WebApi.Models.Environment> { new ProjectMap.WebApi.Models.Environment(), new ProjectMap.WebApi.Models.Environment(), new ProjectMap.WebApi.Models.Environment(), new ProjectMap.WebApi.Models.Environment(), new ProjectMap.WebApi.Models.Environment(), new ProjectMap.WebApi.Models.Environment() };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetEnvironmentsByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(existingEnvironments);

            // Act
            var result = await _controller.Add(new ProjectMap.WebApi.Models.Environment());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Er kunnen maximaal 3 environments aangemaakt worden.", badRequestResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsCreatedAtRoute_WithNewEnvironment()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var environment = new ProjectMap.WebApi.Models.Environment { Name = "Test" };
            var createdEnvironment = new ProjectMap.WebApi.Models.Environment { Id = Guid.NewGuid(), Name = "Test" };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetEnvironmentsByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(new List<ProjectMap.WebApi.Models.Environment>());
            _mockRepo.Setup(repo => repo.InsertAsync(It.IsAny<ProjectMap.WebApi.Models.Environment>())).ReturnsAsync(createdEnvironment);

            // Act
            var result = await _controller.Add(environment);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("ReadEnvironment", createdAtRouteResult.RouteName);
            Assert.Equal(createdEnvironment.Id, createdAtRouteResult.RouteValues["profielKeuzeId"]);
            Assert.Equal(createdEnvironment, createdAtRouteResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Update_ReturnsNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync<IEnvironmentRepository, ProjectMap.WebApi.Models.Environment>((ProjectMap.WebApi.Models.Environment)null);

            // Act
            var result = await _controller.Update(environmentId, new ProjectMap.WebApi.Models.Environment());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Update_ReturnsOk_WithUpdatedEnvironment()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            var existingEnvironment = new ProjectMap.WebApi.Models.Environment { Id = environmentId };
            var newEnvironment = new ProjectMap.WebApi.Models.Environment { Id = environmentId, Name = "Updated" };
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync(existingEnvironment);
            _mockRepo.Setup(repo => repo.UpdateAsync(newEnvironment)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(environmentId, newEnvironment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(newEnvironment, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync<IEnvironmentRepository, ProjectMap.WebApi.Models.Environment>((ProjectMap.WebApi.Models.Environment)null);

            // Act
            var result = await _controller.Delete(environmentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Delete_ReturnsOk_WhenEnvironmentIsDeleted()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            var existingEnvironment = new ProjectMap.WebApi.Models.Environment { Id = environmentId };
            _mockRepo.Setup(repo => repo.ReadAsync(environmentId)).ReturnsAsync(existingEnvironment);
            _mockRepo.Setup(repo => repo.DeleteAsync(environmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(environmentId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
