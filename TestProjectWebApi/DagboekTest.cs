//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using ProjectMap.WebApi.Controllers; // Ensure this matches the actual namespace
//using ProjectMapGroepsproject.WebApi.Models;
//using Xunit;
//using Assert = Xunit.Assert;

//namespace TestProjectWebApi
//{
//    [TestClass]
//    public class DagboekControllerTests
//    {
//        private readonly Mock<Microsoft.Extensions.Logging.ILogger<DagboekController>> _mockLogger;
//        private readonly DagboekController _controller;

//        public DagboekControllerTests()
//        {
//            _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<DagboekController>>();
//            _controller = new DagboekController(_mockLogger.Object);
//        }

//        //[TestMethod]
//        //[Fact]
//        //public void GetAll_ReturnsOkResult_WithListOfDagboeken()
//        //{
//        //    // Act
//        //    var result = _controller.GetAll();

//        //    // Assert
//        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
//        //    var dagboeken = Assert.IsType<List<Dagboek>>(okResult.Value);
//        //    Assert.Empty(dagboeken);
//        //}

//        [TestMethod]
//        [Fact]
//        public void Get_ReturnsNotFound_WhenDagboekDoesNotExist()
//        {
//            // Act
//            var result = _controller.Get(Guid.NewGuid());

//            // Assert
//            Assert.IsType<NotFoundResult>(result.Result);
//        }

//        [TestMethod]
//        [Fact]
//        public void Get_ReturnsOkResult_WithDagboek_WhenDagboekExists()
//        {
//            // Arrange
//            var dagboek = new Dagboek { Id = Guid.NewGuid() };
//            _controller.Create(dagboek);

//            // Act
//            var result = _controller.Get(dagboek.Id);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnedDagboek = Assert.IsType<Dagboek>(okResult.Value);
//            Assert.Equal(dagboek.Id, returnedDagboek.Id);
//        }

//        [TestMethod]
//        [Fact]
//        public void Create_ReturnsCreatedAtActionResult_WithDagboek()
//        {
//            // Arrange
//            var dagboek = new Dagboek();

//            // Act
//            var result = _controller.Create(dagboek);

//            // Assert
//            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
//            var createdDagboek = Assert.IsType<Dagboek>(createdAtActionResult.Value);
//            Assert.NotEqual(Guid.Empty, createdDagboek.Id);
//        }

//        [TestMethod]
//        [Fact]
//        public void Update_ReturnsNotFound_WhenDagboekDoesNotExist()
//        {
//            // Arrange
//            var updatedDagboek = new Dagboek { Id = Guid.NewGuid() };

//            // Act
//            var result = _controller.Update(updatedDagboek.Id, updatedDagboek);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [TestMethod]
//        [Fact]
//        public void Update_ReturnsOkResult_WithUpdatedDagboek_WhenDagboekExists()
//        {
//            // Arrange
//            var dagboek = new Dagboek { Id = Guid.NewGuid() };
//            _controller.Create(dagboek);
//            var updatedDagboek = new Dagboek { Id = dagboek.Id, DagboekBladzijde1 = "Updated" };

//            // Act
//            var result = _controller.Update(dagboek.Id, updatedDagboek);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnedDagboek = Assert.IsType<Dagboek>(okResult.Value);
//            Assert.Equal(updatedDagboek.DagboekBladzijde1, returnedDagboek.DagboekBladzijde1);
//        }

//        [TestMethod]
//        [Fact]
//        public void Delete_ReturnsNotFound_WhenDagboekDoesNotExist()
//        {
//            // Act
//            var result = _controller.Delete(Guid.NewGuid());

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [TestMethod]
//        [Fact]
//        public void Delete_ReturnsNoContent_WhenDagboekExists()
//        {
//            // Arrange
//            var dagboek = new Dagboek { Id = Guid.NewGuid() };
//            _controller.Create(dagboek);

//            // Act
//            var result = _controller.Delete(dagboek.Id);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }
//    }
//}
