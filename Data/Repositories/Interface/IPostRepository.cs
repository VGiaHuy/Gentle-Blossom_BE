using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllAsync();

    }
}
