using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Airplane.Best.Routes.Api.UnitTests.Repository
{
    public class RouteRepositoryTest
    {
        private readonly Mock<IMemoryContext> _memoryContextMock;
        private readonly RouteRepository _routeRepository;

        public RouteRepositoryTest()
        {
            _memoryContextMock = new Mock<IMemoryContext>();
            _routeRepository = new RouteRepository(_memoryContextMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRoutes()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B" },
                new Route { Id = Guid.NewGuid(), OriginName = "C", DestinationName = "D" }
            };

            var mockRoutesDbSet = routes.AsQueryable().BuildMockDbSet();

            _memoryContextMock.Setup(m => m.Routes).Returns(mockRoutesDbSet.Object);

            // Act
            var result = await _routeRepository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("A", result[0].OriginName);
        }

        [Fact]
        public async Task GetRangeAsync_ShouldReturnFilteredRoutes()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B", Value = 30 },
                new Route { Id = Guid.NewGuid(), OriginName = "C", DestinationName = "D", Value = 10 }
            };

            var mockRoutesDbSet = routes.AsQueryable().BuildMockDbSet();
            _memoryContextMock.Setup(m => m.Routes).Returns(mockRoutesDbSet.Object);

            // Act
            var result = await _routeRepository.GetRangeAsync("A", "B", CancellationToken.None);

            // Assert
            Assert.True(result.Count == 2);
            Assert.Equal("A", result[0].OriginName);
            Assert.Equal(30, result[1].Value);
        }

        [Fact]
        public async Task GetBestAsync_ShouldReturnBestRoute()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B", Value = 40 },
                new Route { Id = Guid.NewGuid(), OriginName = "A", DestinationName = "B", Value = 30 },
                new Route { Id = Guid.NewGuid(), OriginName = "C", DestinationName = "D", Value = 10 }
            };

            var mockRoutesDbSet = routes.AsQueryable().BuildMockDbSet();
            _memoryContextMock.Setup(m => m.Routes).Returns(mockRoutesDbSet.Object);

            // Act
            var result = await _routeRepository.GetBestAsync("A", "B", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(30, result.Value);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRouteAndConnections()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var oldConnectionId = Guid.NewGuid();

            var existingRoute = new Route
            {
                Id = routeId,
                OriginName = "GRU",
                DestinationName = "BRC",
                IsAvaiable = true,
                Value = 50,
                Connections = new List<Connection>
                {
                    new Connection { Id = oldConnectionId, Name = "SCL", RouteId = routeId }
                }
            };

            var updatedRoute = new Route
            {
                OriginName = "GRU",
                DestinationName = "CDG",
                Value = 99,
                Connections = new List<Connection>
                {
                    new Connection { Name = "ORL" },
                    new Connection { Name = "CDG" }
                }
            };

            // Simula retorno de Routes como IQueryable para permitir buscas com LINQ
            var routeQuery = new List<Route> { existingRoute }.AsQueryable();
            var mockRouteDbSet = routeQuery.BuildMockDbSet();


            var mockConnectionDbSet = new Mock<DbSet<Connection>>();
            mockConnectionDbSet.Setup(m => m.Remove(It.IsAny<Connection>()));
            mockConnectionDbSet.Setup(m => m.Add(It.IsAny<Connection>()));

            _memoryContextMock.Setup(m => m.Routes).Returns(mockRouteDbSet.Object);
            _memoryContextMock.Setup(m => m.Connections).Returns(mockConnectionDbSet.Object);
            _memoryContextMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _routeRepository.UpdateAsync(routeId, updatedRoute);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CDG", result.DestinationName);
            Assert.Equal(99, result.Value);
            Assert.Equal(2, result.Connections.Count);
            Assert.All(result.Connections, c => Assert.NotEqual(Guid.Empty, c.Id));
            Assert.All(result.Connections, c => Assert.Equal(routeId, c.RouteId));
            Assert.Contains(result.Connections, c => c.Name == "ORL");
            Assert.Contains(result.Connections, c => c.Name == "CDG");

            mockConnectionDbSet.Verify(m => m.Remove(It.Is<Connection>(c => c.Id == oldConnectionId)), Times.Once);
            mockConnectionDbSet.Verify(m => m.Add(It.Is<Connection>(c => c.Name == "ORL")), Times.Once);
            mockConnectionDbSet.Verify(m => m.Add(It.Is<Connection>(c => c.Name == "CDG")), Times.Once);
            _memoryContextMock.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRouteAndReturnTrue_WhenRouteExists()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            var routeToDelete = new Route
            {
                Id = routeId,
                OriginName = "GRU",
                DestinationName = "CDG",
                IsAvaiable = true,
                Value = 75
            };

            // Simula retorno de Routes como IQueryable para permitir buscas com LINQ
            var routeQuery = new List<Route> { routeToDelete }.AsQueryable();
            var mockRouteDbSet = routeQuery.BuildMockDbSet();

            _memoryContextMock.Setup(m => m.Routes).Returns(mockRouteDbSet.Object);
            _memoryContextMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);
            mockRouteDbSet.Setup(m => m.Remove(It.IsAny<Route>()));

            // Act
            var result = await _routeRepository.DeleteAsync(routeId);

            // Assert
            Assert.True(result);
            mockRouteDbSet.Verify(m => m.Remove(It.Is<Route>(r => r.Id == routeId)), Times.Once);
            _memoryContextMock.Verify(m => m.SaveChangesAsync(), Times.Once);
        }
    }
}
