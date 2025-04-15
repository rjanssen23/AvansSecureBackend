//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;
//using ProjectMap.WebApi.Controllers;
//using ProjectMap.WebApi.Repositories;
//using ProjectMapGroepsproject.WebApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using Assert = Xunit.Assert;

//namespace ProjectMapGroepsProject.WebApi.Tests.Controllers
//{
//    [TestClass]
//    public class AgendaControllerTests
//    {
//        private readonly Mock<IAgendaRepository> _mockRepository;
//        private readonly Mock<ILogger<AgendaController>> _mockLogger;
//        private readonly AgendaController _controller;

//        public AgendaControllerTests()
//        {
//            _mockRepository = new Mock<IAgendaRepository>();
//            _mockLogger = new Mock<ILogger<AgendaController>>();
//            _controller = new AgendaController(_mockRepository.Object, _mockLogger.Object);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task GetAll_ReturnsOkResult_WithListOfAgendas()
//        {
//            // Arrange
//            var agendas = new List<Agenda> { new Agenda { Id = Guid.NewGuid() } };
//            _mockRepository.Setup(repo => repo.ReadAsync()).ReturnsAsync(agendas);

//            // Act
//            var result = await _controller.GetAll();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnValue = Assert.IsType<List<Agenda>>(okResult.Value);
//            Assert.Equal(agendas.Count, returnValue.Count);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Get_ReturnsOkResult_WithAgenda()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            var agenda = new Agenda { Id = agendaId };
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync(agenda);

//            // Act
//            var result = await _controller.Get(agendaId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnValue = Assert.IsType<Agenda>(okResult.Value);
//            Assert.Equal(agendaId, returnValue.Id);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Get_ReturnsNotFound_WhenAgendaNotFound()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync((Agenda)null);

//            // Act
//            var result = await _controller.Get(agendaId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result.Result);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Create_ReturnsCreatedAtAction_WithCreatedAgenda()
//        {
//            // Arrange
//            var agenda = new Agenda { Id = Guid.NewGuid() };
//            _mockRepository.Setup(repo => repo.InsertAsync(agenda)).ReturnsAsync(agenda);

//            // Act
//            var result = await _controller.Create(agenda);

//            // Assert
//            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
//            var returnValue = Assert.IsType<Agenda>(createdAtActionResult.Value);
//            Assert.Equal(agenda.Id, returnValue.Id);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Update_ReturnsNoContent_WhenAgendaUpdated()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            var agenda = new Agenda { Id = agendaId };
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync(agenda);
//            _mockRepository.Setup(repo => repo.UpdateAsync(agenda)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.Update(agendaId, agenda);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Update_ReturnsNotFound_WhenAgendaNotFound()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            var agenda = new Agenda { Id = agendaId };
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync((Agenda)null);

//            // Act
//            var result = await _controller.Update(agendaId, agenda);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Delete_ReturnsNoContent_WhenAgendaDeleted()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            var agenda = new Agenda { Id = agendaId };
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync(agenda);
//            _mockRepository.Setup(repo => repo.DeleteAsync(agendaId)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.Delete(agendaId);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }
//        [TestMethod]
//        [Fact]
//        public async Task Delete_ReturnsNotFound_WhenAgendaNotFound()
//        {
//            // Arrange
//            var agendaId = Guid.NewGuid();
//            _mockRepository.Setup(repo => repo.ReadAsync(agendaId)).ReturnsAsync((Agenda)null);

//            // Act
//            var result = await _controller.Delete(agendaId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }
//    }
//}
