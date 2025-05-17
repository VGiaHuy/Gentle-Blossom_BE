using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllAsync(int page, int pageSize);
        Task<List<Post>> GetPostsOfUserById(int id, int page, int pageSize);

    }
}
