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
    public class ProfielKeuzeControllerTests
    {
        private readonly Mock<IProfielKeuzeRepository> _mockRepo;
        private readonly Mock<ILogger<ProfielKeuzeController>> _mockLogger;
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly ProfielKeuzeController _controller;

        public ProfielKeuzeControllerTests()
        {
            _mockRepo = new Mock<IProfielKeuzeRepository>();
            _mockLogger = new Mock<ILogger<ProfielKeuzeController>>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _controller = new ProfielKeuzeController(_mockRepo.Object, _mockLogger.Object, _mockAuthService.Object);
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
            var profielKeuzes = new List<ProfielKeuze> { new ProfielKeuze { Id = Guid.NewGuid() } };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetProfielKeuzesByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(profielKeuzes);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(profielKeuzes, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Get_WithId_ReturnsNotFound_WhenProfielKeuzeDoesNotExist()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync((ProfielKeuze)null);

            // Act
            var result = await _controller.Get(profielKeuzeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [TestMethod]
        [Fact]
        public async Task Get_WithId_ReturnsOk_WithProfielKeuze()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            var profielKeuze = new ProfielKeuze { Id = profielKeuzeId };
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync(profielKeuze);

            // Act
            var result = await _controller.Get(profielKeuzeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(profielKeuze, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns((string)null);

            // Act
            var result = await _controller.Add(new ProfielKeuze());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsBadRequest_WhenUserHasMaxProfielKeuzes()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var existingProfielen = new List<ProfielKeuze> { new ProfielKeuze(), new ProfielKeuze(), new ProfielKeuze(), new ProfielKeuze(), new ProfielKeuze(), new ProfielKeuze() };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetProfielKeuzesByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(existingProfielen);

            // Act
            var result = await _controller.Add(new ProfielKeuze());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Er kunnen maximaal 6 profielen aangemaakt worden.", badRequestResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Add_ReturnsCreatedAtRoute_WithNewProfielKeuze()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var profielKeuze = new ProfielKeuze { Name = "Test" };
            var createdProfielKeuze = new ProfielKeuze { Id = Guid.NewGuid(), Name = "Test" };
            _mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockRepo.Setup(repo => repo.GetProfielKeuzesByUserIdAsync(Guid.Parse(userId))).ReturnsAsync(new List<ProfielKeuze>());
            _mockRepo.Setup(repo => repo.InsertAsync(It.IsAny<ProfielKeuze>())).ReturnsAsync(createdProfielKeuze);

            // Act
            var result = await _controller.Add(profielKeuze);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("ReadProfielKeuze", createdAtRouteResult.RouteName);
            Assert.Equal(createdProfielKeuze.Id, createdAtRouteResult.RouteValues["profielKeuzeId"]);
            Assert.Equal(createdProfielKeuze, createdAtRouteResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Update_ReturnsNotFound_WhenProfielKeuzeDoesNotExist()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync((ProfielKeuze)null);

            // Act
            var result = await _controller.Update(profielKeuzeId, new ProfielKeuze());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Update_ReturnsOk_WithUpdatedProfielKeuze()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            var existingProfielKeuze = new ProfielKeuze { Id = profielKeuzeId };
            var newProfielKeuze = new ProfielKeuze { Id = profielKeuzeId, Name = "Updated" };
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync(existingProfielKeuze);
            _mockRepo.Setup(repo => repo.UpdateAsync(newProfielKeuze)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(profielKeuzeId, newProfielKeuze);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(newProfielKeuze, okResult.Value);
        }

        [TestMethod]
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProfielKeuzeDoesNotExist()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync((ProfielKeuze)null);

            // Act
            var result = await _controller.Delete(profielKeuzeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        [Fact]
        public async Task Delete_ReturnsOk_WhenProfielKeuzeIsDeleted()
        {
            // Arrange
            var profielKeuzeId = Guid.NewGuid();
            var existingProfielKeuze = new ProfielKeuze { Id = profielKeuzeId };
            _mockRepo.Setup(repo => repo.ReadAsync(profielKeuzeId)).ReturnsAsync(existingProfielKeuze);
            _mockRepo.Setup(repo => repo.DeleteAsync(profielKeuzeId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(profielKeuzeId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
