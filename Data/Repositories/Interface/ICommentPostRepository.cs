using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface ICommentPostRepository : IGenericRepository<CommentPost>
    {
        Task<(List<CommentPost>, bool)> GetCommentsByPostIdAsync(int postId, int page, int pageSize);
    }
}
