using AuthCookbook.Core.Shared.Plugins;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Services.Shared.UnitOfWork
{
    public class UnitOfWork(InMemoryDbContext dbContext) : IUnitOfWork<InMemoryDbContext>
    {

        public void Commit()
        {
            dbContext.SaveChanges();
        }
        public InMemoryDbContext GetDbContext => dbContext;

        public List<Dictionary<string, object>> ExecuteSQLSelectQuery(string sql)
        {
            var connection = dbContext.Database.GetDbConnection();
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                using var result = command.ExecuteReader();
                var entities = new List<Dictionary<string, object>>();
                while (result.Read())
                {
                    var entity = new Dictionary<string, object>();
                    for (var i = 0; i < result.FieldCount; i++)
                    {
                        entity[result.GetName(i)] = result.GetValue(i);
                    }
                    entities.Add(entity);
                }
                return entities;
            }
            finally
            {
                dbContext.Database.CloseConnection();
            }
        }

        public void OpenConnection()
        {
            try
            {
                dbContext.Database.CanConnect();
                dbContext.Database.BeginTransaction();
            }
            catch
            {
                dbContext.Database.RollbackTransaction();
                throw;
            }
        }

        public void RollbackConnection()
        {
            dbContext.Database.RollbackTransaction();
        }
    }
}
