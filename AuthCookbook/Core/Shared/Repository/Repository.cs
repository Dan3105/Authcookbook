using Microsoft.EntityFrameworkCore;

namespace AuthCookbook.Core.Shared.Repository
{
    public class Repository<T>(DbSet<T> dbset) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbset = dbset;

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public IQueryable<T> Get() => _dbset.AsQueryable();

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }
    }
}
