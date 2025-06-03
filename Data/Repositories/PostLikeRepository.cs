using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<bool> CheckLikePost(int postId, int userId)
        {
            return await _context.PostLikes
                .AnyAsync(pl => pl.PostId == postId && pl.UserId == userId);
        }

        public async Task LikePost(int postId, int userId)
        {
            await _context.PostLikes.AddAsync(new PostLike
            {
                PostId = postId,
                UserId = userId
            });
        }

        public async Task UnLikePost(int postId, int userId)
        {
            var postLike = await _context.PostLikes
                .FirstAsync(pl => pl.PostId == postId && pl.UserId == userId);

            _context.PostLikes.Remove(postLike);
        }
    }
}
