namespace AuthCookbook.Core.Shared.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
        List<Dictionary<string, object>> ExecuteSQLSelectQuery(string sql);
        void OpenConnection();
        void RollbackConnection();
    }
}
