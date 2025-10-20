using Microsoft.EntityFrameworkCore;

namespace AuthServer.Services.Shared.UnitOfWork
{
    public interface IUnitOfWork<T> where T : DbContext
    {
        T GetDbContext { get; }
        void Commit();
        List<Dictionary<string, object>> ExecuteSQLSelectQuery(string sql);
        void OpenConnection();
        void RollbackConnection();
    }
}
