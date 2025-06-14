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

        public async Task DeleteRangeByPostIdAsync(int postId)
        {
            var commentPosts = await _context.CommentPosts
                .Where(pm => pm.PostId == postId)
                .ToListAsync();

            if (commentPosts.Any())
            {
                _context.CommentPosts.RemoveRange(commentPosts);
            }
        }

        public async Task<(List<CommentPost>, bool)> GetCommentsByPostIdAsync(int postId, int page, int pageSize)
        {
            var query = _context.CommentPosts
                .Where(p => p.PostId == postId)
                .OrderByDescending(p => p.CommentDate)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.UserType)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.Expert)
                .AsNoTracking();

            // Tính tổng số bình luận để kiểm tra hasMore
            var totalComments = await query.CountAsync();
            var hasMore = totalComments > (page * pageSize);

            // Áp dụng phân trang
            var comments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (comments, hasMore);
        }
    }
}
