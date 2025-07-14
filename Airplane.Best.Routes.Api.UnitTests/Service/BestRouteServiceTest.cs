using Airplane.Best.Routes.Application.Services;
using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Airplane.Best.Routes.Domain.Models;
using Mapster;
using Moq;

namespace Airplane.Best.Routes.Api.UnitTests.Service
{
    public class BestRouteServiceTest
    {
        private readonly Mock<IRouteRepository> _routeRepositoryMock;
        private readonly Mock<IConnectionRepository> _connectionRepositoryMock;
        private readonly BestRouteService _bestRouteService;

        public BestRouteServiceTest()
        {
            _routeRepositoryMock = new Mock<IRouteRepository>();
            _connectionRepositoryMock = new Mock<IConnectionRepository>();
            _bestRouteService = new BestRouteService(_routeRepositoryMock.Object, _connectionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllRoutesAsync_ShouldReturnAllRoutes()
        {
            // Arrange
            var expectedRoutes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 30 }
            };

            _routeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRoutes);

            // Act
            var result = await _bestRouteService.GetAllRoutesAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRoutes.Count, result.Count);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnBestRoute()
        {
            // Arrange
            var request = new GetRoutesModel { Origin = "GRU", Destination = "CDG" };
            var expectedRoute = new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B" };

            _routeRepositoryMock.Setup(repo => repo.GetBestAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRoute);

            // Act
            var result = await _bestRouteService.GetBestRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRoute.Id, result.Id);
        }

        [Fact]
        public async Task GetRangeRoutesAsync_ShouldReturnRangeOfRoutes()
        {
            // Arrange
            var request = new GetRoutesModel { Origin = "GRU", Destination = "CDG" };
            var expectedRoutes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG" , Value = 30},
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG" , Value = 80}
            };

            _routeRepositoryMock.Setup(repo => repo.GetRangeAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRoutes);

            // Act
            var result = await _bestRouteService.GetRangeRoutesAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRoutes.Count, result.Count);
        }

        [Fact]
        public async Task CreateRouteAsync_ShouldCreateAndReturnNewRoute()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new CreateRouteModel
            {
                IsAvaiable = true,
                OriginName = "GRU",
                DestinationName = "RGT",
                Value = 100,
                Connections = new List<CreateConnectionModel> { new CreateConnectionModel { Name = "OPR" } }
            };

            var expectedRoute = request.Adapt<Route>();
            expectedRoute.Id = Guid.NewGuid();
            expectedRoute.Connections.ForEach(c => c.Id = Guid.NewGuid());

            _routeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Route>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRoute);

            // Act
            var result = await _bestRouteService.CreateRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRoute.Id, result.Id);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldUpdateAndReturnRoute()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new UpdateRouteModel
            {
                Id = routeId,
                IsAvaiable = true,
                OriginName = "GRU",
                DestinationName = "RGT",
                Value = 100
            };

            var existingRoute = new Route
            {
                Id = routeId,
                OriginName = "GRU",
                DestinationName = "RGT",
                IsAvaiable = false,
                Value = 50,
                Connections = new List<Connection>
                {
                    new Connection { Id = Guid.NewGuid(), RouteId = routeId }
                }
            };

            _routeRepositoryMock.Setup(repo => repo.GetByIdAsync(routeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRoute);

            _connectionRepositoryMock.Setup(repo => repo.ClearConnectionsByRouteId(routeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _routeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Route>()))
                .ReturnsAsync((Route r) => r);

            // Act
            var result = await _bestRouteService.UpdateRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.OriginName, result.OriginName);
            Assert.Equal(request.DestinationName, result.DestinationName);
            Assert.Equal(request.IsAvaiable, result.IsAvaiable);
            Assert.Equal(request.Value, result.Value);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldReturnNullIfRouteNotFound()
        {
            // Arrange
            var request = new UpdateRouteModel { Id = Guid.NewGuid() };
            _routeRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route)null);

            // Act
            var result = await _bestRouteService.UpdateRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteRouteAsync_ShouldDeleteRoute()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var routeToDelete = new Route { Id = routeId };

            _routeRepositoryMock.Setup(repo => repo.GetByIdAsync(routeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(routeToDelete);

            _routeRepositoryMock.Setup(repo => repo.DeleteAsync(routeToDelete))
                .ReturnsAsync(true);

            // Act
            var result = await _bestRouteService.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result);
        }
    }
}
