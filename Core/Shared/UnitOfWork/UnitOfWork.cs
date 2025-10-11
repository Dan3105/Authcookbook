using AuthCookbook.Core.Plugins;
using Microsoft.EntityFrameworkCore;

namespace AuthCookbook.Core.Shared.UnitOfWork
{
    public class UnitOfWork(InMemoryDbContext dbContext) : IUnitOfWork
    {
        private readonly InMemoryDbContext _dbContext = dbContext;
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public List<Dictionary<string, object>> ExecuteSQLSelectQuery(string sql)
        {
            var connection = _dbContext.Database.GetDbConnection();
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
                _dbContext.Database.CloseConnection();
            }
        }

        public void OpenConnection()
        {
            try
            {
                _dbContext.Database.CanConnect();
                _dbContext.Database.BeginTransaction();
            }
            catch
            {
                _dbContext.Database.RollbackTransaction();
                throw;
            }
        }

        public void RollbackConnection()
        {
            _dbContext.Database.RollbackTransaction();
        }
    }
}

