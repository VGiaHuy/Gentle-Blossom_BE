using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface ICommentPostRepository : IGenericRepository<CommentPost>
    {
        Task<List<CommentPost>> GetCommentsByPostIdAsync(int postId);
    }
}
