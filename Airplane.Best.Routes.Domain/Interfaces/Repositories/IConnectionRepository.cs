namespace Airplane.Best.Routes.Domain.Interfaces.Repositories
{
    public interface IConnectionRepository
    {
        Task<bool> ClearConnectionsByRouteId(Guid routeId, CancellationToken cancellation);
    }
}
