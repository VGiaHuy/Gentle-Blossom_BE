using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostMediaRepository : IGenericRepository<PostMedium>
    {
        Task DeleteRangeByPostIdAsync(int postId);
        Task DeleteRangeByMediaUrlAsync(string url, int postId);
    }
}
