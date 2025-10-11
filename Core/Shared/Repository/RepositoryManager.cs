using AuthCookbook.Core.Plugins;

namespace AuthCookbook.Core.Shared.Repository
{
    public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : class;
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly InMemoryDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public RepositoryManager(InMemoryDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var dbSet = _context.Set<T>();
                var repositoryInstance = new Repository<T>(dbSet);
                _repositories[type] = repositoryInstance;
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}
