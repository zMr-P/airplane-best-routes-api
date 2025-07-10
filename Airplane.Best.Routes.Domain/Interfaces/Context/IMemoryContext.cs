using Airplane.Best.Routes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airplane.Best.Routes.Domain.Interfaces.Context
{
    public interface IMemoryContext
    {
        DbSet<Route> Routes { get; set; }
        DbSet<Connection> Connections { get; set; }
        Task<int> SaveChangesAsync();
    }
}

