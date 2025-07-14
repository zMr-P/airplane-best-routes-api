using Airplane.Best.Routes.Domain.Entities;
using Airplane.Best.Routes.Domain.Interfaces.Context;
using Airplane.Best.Routes.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Airplane.Best.Routes.Api.UnitTests.Repository
{
    public class ConnectionRepositoryTest
    {
        private readonly Mock<IMemoryContext> _memoryContextMock;
        private readonly Mock<ILogger<ConnectionRepository>> _loggerMock;
        private readonly ConnectionRepository _connectionRepository;

        public ConnectionRepositoryTest()
        {
            _memoryContextMock = new Mock<IMemoryContext>();
            _loggerMock = new Mock<ILogger<ConnectionRepository>>();
            _connectionRepository = new ConnectionRepository(_memoryContextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ClearConnectionsByRouteId_ShouldRemoveConnectionsAndReturnTrue()
        {
            // Arrange
            var routeId = Guid.NewGuid();

            var connections = new List<Connection>
            {
                new Connection { Id = Guid.NewGuid(), RouteId = routeId, Name = "C1" },
                new Connection { Id = Guid.NewGuid(), RouteId = routeId, Name = "C2" }
            };

            var connectionsMock = connections.AsQueryable().BuildMockDbSet();

            _memoryContextMock.Setup(c => c.Connections).Returns(connectionsMock.Object);

            // Act
            var result = await _connectionRepository.ClearConnectionsByRouteId(routeId, CancellationToken.None);

            // Assert
            Assert.True(result);
            connectionsMock.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<Connection>>()), Times.Once);
            _memoryContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ClearConnectionsByRouteId_ShouldReturnFalse_WhenExceptionOccurs()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            _memoryContextMock.Setup(c => c.Connections)
                .Throws(new Exception("Database error"));

            // Act
            var result = await _connectionRepository.ClearConnectionsByRouteId(routeId, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

    }
}
