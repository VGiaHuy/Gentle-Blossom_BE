using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task<bool> CheckLikePost(int postId, int userId);
        Task LikePost(int postId, int userId);
        Task UnLikePost(int postId, int userId);

    }
}
