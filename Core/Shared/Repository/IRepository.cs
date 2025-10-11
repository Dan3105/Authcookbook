namespace AuthCookbook.Core.Shared.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
