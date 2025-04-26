namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity); // Không cần async
        void Delete(T entity); // Không cần async
    }

}
