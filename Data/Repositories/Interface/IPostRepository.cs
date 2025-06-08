using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<PostDTO>> GetAllAsync(int userId, int page, int pageSize);
        Task<List<PostDTO>> GetPostsOfUserById(int id, int page, int pageSize);
        Task<bool> ToggleLikePost(int posId, int handle);
        Task<PostDTO> GetPostByIdAsync(int id);
    }
}
