using AuthCookbook.Core.Shared.Plugins;
using AuthCookbook.Core.Shared.UnitOfWork;

namespace AuthCookbook.Core.Shared.Repository
{
    public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : class;
        void SaveChanges();
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly InMemoryDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryManager(InMemoryDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _unitOfWork = unitOfWork;
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

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
