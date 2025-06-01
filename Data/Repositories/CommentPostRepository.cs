using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class CommentPostRepository : GenericRepository<CommentPost>, ICommentPostRepository
    {
        public CommentPostRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<CommentPost>> GetCommentsByPostIdAsync(int postId)
        {

            return await _context.CommentPosts
                .Where(p => p.PostId == postId)
                .OrderByDescending(p => p.CommentDate)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.UserType)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.Expert)
                .AsNoTracking() // Tăng hiệu suất
                .ToListAsync();
        }
    }
}
