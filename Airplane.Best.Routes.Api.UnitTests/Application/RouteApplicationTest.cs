using Airplane.Best.Routes.Application.Application;
using Airplane.Best.Routes.Application.Dtos.RouteService.Request;
using Airplane.Best.Routes.Application.Dtos.RouteService.Response;
using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Services;
using Airplane.Best.Routes.Domain.Messages;
using Airplane.Best.Routes.Domain.Models;
using Mapster;
using Moq;
using Xunit.Sdk;

namespace Airplane.Best.Routes.Api.UnitTests.Application
{
    public class RouteApplicationTest
    {
        private readonly Mock<IBestRouteService> _bestRouteServiceMock;
        private readonly BestRoutesApplication _bestRoutesApplication;

        public RouteApplicationTest()
        {
            _bestRouteServiceMock = new Mock<IBestRouteService>();
            _bestRoutesApplication = new BestRoutesApplication(_bestRouteServiceMock.Object);
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

            _bestRouteServiceMock.Setup(repo => repo.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(routes);

            // Act
            var result = await _bestRoutesApplication.GetAllRoutesAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count);
        }

        [Fact]
        public async Task GetAllRoutesAsync_ShouldReturnError_WhenNoRoutesExist()
        {
            // Arrange
            _bestRouteServiceMock.Setup(repo => repo.GetAllRoutesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Route>());

            // Act
            var result = await _bestRoutesApplication.GetAllRoutesAsync(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Empty(result.Data);
            Assert.Contains(ErrorMessages.NoRoutesFound, result.ErrorMessages);
        }

        [Fact]
        public async Task GetRangeRoutesAsync_ShouldReturnRoutes_WhenRoutesExist()
        {
            // Arrange
            var request = new GetRoutesRequestDto { Origin = "GRU", Destination = "CDG" };
            var requestModel = request.Adapt<GetRoutesModel>();

            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value  = 30},
            };

            _bestRouteServiceMock.Setup(repo => repo.GetRangeRoutesAsync(It.IsAny<GetRoutesModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(routes);

            // Act
            var result = await _bestRoutesApplication.GetRangeRoutesAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetRangeRoutesAsync_ShouldReturnError_WhenNoRoutesExist()
        {
            // Arrange
            var request = new GetRoutesRequestDto { Origin = "GRU", Destination = "CDG" };
            var requestModel = request.Adapt<GetRoutesModel>();

            _bestRouteServiceMock.Setup(repo => repo.GetRangeRoutesAsync(requestModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Route>());

            // Act
            var result = await _bestRoutesApplication.GetRangeRoutesAsync(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Empty(result.Data);
            Assert.Contains(ErrorMessages.NoRoutesFound, result.ErrorMessages);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnBestRoute_WhenRouteExists()
        {
            // Arrange
            var request = new GetRoutesRequestDto { Origin = "GRU", Destination = "CDG" };

            var bestRoute = new Route { Id = Guid.NewGuid(), OriginName = "GRU", DestinationName = "CDG", Value = 30 };
            var bestRouteDto = bestRoute.Adapt<GetRouteResponseDto>();

            _bestRouteServiceMock.Setup(repo => repo.GetBestRouteAsync(It.IsAny<GetRoutesModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bestRoute);

            // Act
            var result = await _bestRoutesApplication.GetBestRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equivalent(bestRouteDto, result.Data);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnError_WhenNoRouteExists()
        {
            // Arrange
            var request = new GetRoutesRequestDto { Origin = "GRU", Destination = "CDG" };
            var requestModel = request.Adapt<GetRoutesModel>();

            _bestRouteServiceMock.Setup(repo => repo.GetBestRouteAsync(requestModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _bestRoutesApplication.GetBestRouteAsync(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains(ErrorMessages.NoRoutesFound, result.ErrorMessages);
        }

        [Fact]
        public async Task CreateRouteAsync_ShouldReturnCreatedRoute_WhenSuccessful()
        {
            // Arrange
            var request = new CreateRouteRequestDto
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequestDto>
                {
                    new CreateConnectionRequestDto { Name = "SCL" }
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
            var responseRoute = newRoute.Adapt<GetRouteResponseDto>();

            _bestRouteServiceMock.Setup(repo => repo.CreateRouteAsync(It.IsAny<CreateRouteModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newRoute);

            // Act
            var result = await _bestRoutesApplication.CreateRouteAsync(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equivalent(responseRoute, result.Data);
        }

        [Fact]
        public async Task CreateRouteAsync_ShouldReturnError_WhenCreationFails()
        {
            // Arrange
            var request = new CreateRouteRequestDto
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequestDto>
                {
                    new CreateConnectionRequestDto { Name = "SCL" }
                }
            };

            _bestRouteServiceMock.Setup(repo => repo.CreateRouteAsync(It.IsAny<CreateRouteModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _bestRoutesApplication.CreateRouteAsync(request, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains(ErrorMessages.RouteCreationFailed, result.ErrorMessages);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldReturnUpdatedRoute_WhenSuccessful()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new UpdateRouteRequestDto
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequestDto>
                {
                    new CreateConnectionRequestDto { Name = "SCL" }
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

            var responseRoute = updatedRoute.Adapt<GetRouteResponseDto>();

            _bestRouteServiceMock.Setup(repo => repo.UpdateRouteAsync(It.IsAny<UpdateRouteModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedRoute);

            // Act
            var result = await _bestRoutesApplication.UpdateRouteAsync(routeId, request, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equivalent(responseRoute, result.Data);
        }

        [Fact]
        public async Task UpdateRouteAsync_ShouldReturnError_WhenUpdateFails()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var request = new UpdateRouteRequestDto
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 30,
                Connections = new List<CreateConnectionRequestDto>
                {
                    new CreateConnectionRequestDto { Name = "SCL" }
                }
            };
            var requestModel = request.Adapt<UpdateRouteModel>();

            _bestRouteServiceMock.Setup(repo => repo.UpdateRouteAsync(requestModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Route?)null);

            // Act
            var result = await _bestRoutesApplication.UpdateRouteAsync(routeId, request, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
            Assert.Contains(ErrorMessages.RouteNotFound, result.ErrorMessages);
        }

        [Fact]
        public async Task DeleteRouteAsync_ShouldReturnSuccessOutput_WhenRouteIsDeleted()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            _bestRouteServiceMock.Setup(r => r.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _bestRoutesApplication.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains(SuccessMessages.RouteDeleted, result.Messages);
            Assert.Empty(result.ErrorMessages);

            _bestRouteServiceMock.Verify(r => r.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRouteAsync_ShouldReturnErrorOutput_WhenRouteDoesNotExist()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            _bestRouteServiceMock.Setup(r => r.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _bestRoutesApplication.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(ErrorMessages.RouteNotFound, result.ErrorMessages);
            Assert.Empty(result.Messages);
            _bestRouteServiceMock.Verify(r => r.DeleteRouteAsync(routeId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
