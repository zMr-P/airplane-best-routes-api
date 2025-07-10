using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Services;
using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Repositories;
using Moq;

namespace Airplane.Best.Routes.Api.UnitTests.Service
{
    public class RouteServiceTest
    {
        private readonly Mock<IRouteRepository> _routeRepositoryMock;
        private readonly RouteService _routeService;

        public RouteServiceTest()
        {
            _routeRepositoryMock = new Mock<IRouteRepository>();
            _routeService = new RouteService(_routeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllRoutesAsync_ShouldReturnRoutes_WhenRoutesExist()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value  = 30},
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "SCL", Value  = 50}
            };

            _routeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(routes);

            // Act
            var result = await _routeService.GetAllRoutesAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count);
        }

        [Fact]
        public async Task GetAllRoutesAsync_ShouldReturnError_WhenNoRoutesExist()
        {
            // Arrange
            _routeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Route>());

            // Act
            var result = await _routeService.GetAllRoutesAsync(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Empty(result.Data);
            Assert.Contains("Nenhuma rota encontrada.", result.ErrorMessages);
        }

        [Fact]
        public async Task GetRangeRoutesAsync_ShouldReturnRoutes_WhenRoutesExist()
        {
            // Arrange
            var request = new GetRoutesRequest { Origin = "GRU", Destination = "CDG" };
            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value  = 30},
            };

            _routeRepositoryMock.Setup(repo => repo.GetRangeAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync(routes);

            // Act
            var result = await _routeService.GetRangeRoutesAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetRangeRoutesAsync_ShouldReturnError_WhenNoRoutesExist()
        {
            // Arrange
            var request = new GetRoutesRequest { Origin = "GRU", Destination = "CDG" };

            _routeRepositoryMock.Setup(repo => repo.GetRangeAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Route>());

            // Act
            var result = await _routeService.GetRangeRoutesAsync(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Empty(result.Data);
            Assert.Contains("Nenhuma rota encontrada.", result.ErrorMessages);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnBestRoute_WhenRouteExists()
        {
            // Arrange
            var request = new GetRoutesRequest { Origin = "GRU", Destination = "CDG" };
            var bestRoute = new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 30 };

            _routeRepositoryMock.Setup(repo => repo.GetBestAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bestRoute);

            // Act
            var result = await _routeService.GetBestRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(bestRoute, result.Data);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnError_WhenNoRouteExists()
        {
            // Arrange
            var request = new GetRoutesRequest { Origin = "GRU", Destination = "CDG" };

            _routeRepositoryMock.Setup(repo => repo.GetBestAsync(request.Origin, request.Destination, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _routeService.GetBestRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains("Nenhuma rota encontrada.", result.ErrorMessages);
        }

        [Fact]
        public async Task CreateRouteAsync_ShouldReturnCreatedRoute_WhenSuccessful()
        {
            // Arrange
            var request = new CreateRouteRequest
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequest>
                {
                    new CreateConnectionRequest { Name = "SCL" }
                }
            };

            var newRoute = new Route
            {
                Id = Guid.NewGuid(),
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<Connection> { }
            };

            var connections = new List<Connection>
            {
                new Connection { Id = Guid.NewGuid(), Name = "SCL", RouteId = newRoute.Id }
            };

            newRoute.Connections = connections;

            _routeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Route>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newRoute);

            // Act
            var result = await _routeService.CreateRouteAsync(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newRoute, result.Data);
        }

        [Fact]
        public async Task CreateRouteAsync_ShouldReturnError_WhenCreationFails()
        {
            // Arrange
            var request = new CreateRouteRequest
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequest>
                {
                    new CreateConnectionRequest { Name = "SCL" }
                }
            };

            _routeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Route>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _routeService.CreateRouteAsync(request, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains("Erro ao criar a rota.", result.ErrorMessages);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldReturnUpdatedRoute_WhenSuccessful()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new CreateRouteRequest
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequest>
                {
                    new CreateConnectionRequest { Name = "SCL" }
                }
            };

            var updatedRoute = new Route
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 25,
                Connections = new List<Connection>
                {
                    new Connection { Id = Guid.NewGuid(), Name = "SCL", RouteId = routeId }
                }
            };
            _routeRepositoryMock.Setup(repo => repo.UpdateAsync(routeId, It.IsAny<Route>()))
                .ReturnsAsync(updatedRoute);
            // Act
            var result = await _routeService.UpdateRouteAsync(routeId, request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updatedRoute, result.Data);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldReturnError_WhenUpdateFails()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new CreateRouteRequest
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequest>
                {
                    new CreateConnectionRequest { Name = "SCL" }
                }
            };
            _routeRepositoryMock.Setup(repo => repo.UpdateAsync(routeId, It.IsAny<Route>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _routeService.UpdateRouteAsync(routeId, request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains("Erro ao atualizar a rota. Verifique se a rota existe.", result.ErrorMessages);
        }

        [Fact]
        public async Task DeleteRouteAsync_ShouldReturnSuccessOutput_WhenRouteIsDeleted()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            _routeRepositoryMock.Setup(r => r.DeleteAsync(routeId))
                .ReturnsAsync(true);

            // Act
            var result = await _routeService.DeleteRouteAsync(routeId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("Rota deletada com sucesso.", result.Messages);
            Assert.Empty(result.ErrorMessages);

            _routeRepositoryMock.Verify(r => r.DeleteAsync(routeId), Times.Once);
        }

        [Fact]
        public async Task DeleteRouteAsync_ShouldReturnErrorOutput_WhenRouteDoesNotExist()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            _routeRepositoryMock.Setup(r => r.DeleteAsync(routeId))
                .ReturnsAsync(false);

            // Act
            var result = await _routeService.DeleteRouteAsync(routeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Erro ao deletar a rota. Verifique se a rota existe.", result.ErrorMessages);
            Assert.Empty(result.Messages);
            _routeRepositoryMock.Verify(r => r.DeleteAsync(routeId), Times.Once);
        }
    }
}
